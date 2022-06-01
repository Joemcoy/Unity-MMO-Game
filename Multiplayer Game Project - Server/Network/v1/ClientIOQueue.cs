using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using Base.Data.Interfaces;
using Base.Factories;
using Base.Helpers;
using Network.Data;
using Network.Data.Enums;
using Network.Data.Interfaces;

namespace Network.v1
{
#if UNITY_5
    public class Tuple<T1, T2>
    {
        public T1 Item1 { get; set; }
        public T2 Item2 { get; set; }

        public Tuple(T1 Item1, T2 Item2)
        {
            this.Item1 = Item1;
            this.Item2 = Item2;
        }
    }
#endif

    public class ClientIOQueue : IUpdater, IQueue
    {
        int IUpdater.Interval { get { return SocketConstants.ReadQueueInterval; } }
        ClientSocket Client;
        Random R;

        public ClientIOQueue(ClientSocket Client)
        {
            this.Client = Client;
            R = new Random();
        }
        
        Tuple<int, ISocketPacket> CreateTuple(int Action, ISocketPacket Packet) { return new Tuple<int, ISocketPacket>(Action, Packet); }
        public void EnqueueWrite(ISocketPacket Packet) { QueueFactory.Enqueue(this, CreateTuple(1, Packet)); }
        public void EnqueueRead(ISocketPacket Packet) { QueueFactory.Enqueue(this, CreateTuple(5, Packet)); }

        void IUpdater.Loop()
        {
            try
            {
                if (!Client.IOEnabled) return;

                var Packet = QueueFactory.Dequeue<Tuple<int, ISocketPacket>>(this);
                if (Packet != null)
                {
                    //LoggerFactory.GetLogger(this).LogInfo("IO Event: {0}:{1}", Client.EndPoint, Packet.Item1 == 5 ? "Read" : Packet.Item1 == 1 ? "Send" : "Unknown");
                    switch (Packet.Item1)
                    {
                        case 1:
                            HandleSend(Packet.Item2);
                            break;
                        case 5:
                            HandleRead(Packet.Item2);
                            break;
                        default:
                            Client.Close(DisconnectReason.Unknown);
                            break;
                    }
                }
            }
            catch(Exception ex)
            {
                Client.ErrorCaught(ex, true);
            }
        }

        void HandleSend(ISocketPacket Packet)
        {
            //Client.FirePacketSending(Packet);

            /*var Buffer = new byte[Packet.Length + sizeof(uint)];
            var Flag = R.Next(0, SocketConstants.ChunkFlag);

            Array.Copy(BitConverter.GetBytes(Packet.ID), 0, Buffer, 0, sizeof(uint));
            Array.Copy(Packet.Buffer, 0, Buffer, sizeof(uint), Packet.Length);
            Buffer = RijndaelHelper.Encrypt(Buffer, Convert.ToByte(Flag));

            Client.Stream.WriteByte(SocketConstants.ChunkFlag - SocketConstants.PacketFlag);
            Client.Stream.WriteByte(Convert.ToByte(SocketConstants.ChunkFlag - Flag));
            var Compress = Buffer.Length >= SocketConstants.MinimumToCompress;

            //if (!Compress)
            //{
                Client.Stream.WriteByte(0);
                Client.Stream.Write(BitConverter.GetBytes(Buffer.Length).Select(BufferTransform).ToArray(), 0, sizeof(int));
                Client.Stream.Write(Buffer, 0, Buffer.Length);*/
            /*}
            else
            {
                Client.Stream.WriteByte(1);

                byte[] Compressed = CompressionHelper.GZipCompress(Buffer);

                Client.Stream.Write(BitConverter.GetBytes(Compressed.Length).Select(BufferTransform).ToArray(), 0, sizeof(int));
                Client.Stream.Write(Compressed, 0, Compressed.Length);

                LoggerFactory.GetLogger(this).LogWarning("Compressed from {0} to {1}", Buffer.Length, Compressed.Length);
            }*/

            Client.FirePacketSending(Packet);
            var Header = new byte[sizeof(uint) * 2 + 2];
            Header[0] = SocketConstants.HandshakeFlag;
            Header[1] = SocketConstants.PacketFlag;

            var ID = BitConverter.GetBytes(Packet.ID);
            var LE = BitConverter.GetBytes(Packet.Length);
            for (int i = 0; i < sizeof(uint) * 2; i++)
                Header[i + 2] = i >= 4 ? LE[Math.Abs(4 - i)] : ID[i];

            Client.Stream.Write(Header, 0, Header.Length);
            Client.Stream.Write(Packet.Buffer, 0, Packet.Length);

            Client.FirePacketSent(Packet);
        }

        public byte BufferTransform(byte B)
        {
            return SocketConstants.ChunkFlag >= B ? Convert.ToByte(SocketConstants.ChunkFlag - B) : B;
        }

        void HandleRead(ISocketPacket Packet)
        {
            Client.FirePacketReceived(Packet);
        }

        void IUpdater.Start()
        {
            //LoggerFactory.GetLogger(this).LogInfo("ClientIO:{0} enabled!", Client.EndPoint);
        }

        void IUpdater.End()
        {
            if (Client.IsConnected)
                Client.Close(DisconnectReason.EndOfStream);
        }
    }
}
