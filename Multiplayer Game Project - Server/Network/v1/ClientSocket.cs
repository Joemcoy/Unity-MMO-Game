using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Collections.Generic;

using Base;
using Base.Helpers;
using Base.Factories;
using Base.Data.Interfaces;

using Network.Data;
using Network.Data.Enums;
using Network.Data.EventArgs;
using Network.Data.Interfaces;
using Network.Protocol;
using System.IO;
using System.IO.Compression;
using System.Threading;

namespace Network.v1
{
    public class ClientSocket : IClientSocket, IUpdater
    {
        Socket Client;
        internal object responseLock = new object(), writeLock =new object(), syncLock = new object();

        Dictionary<uint, IResponse> ResponseDict;
        ClientIOQueue IOQueue;
        ClientPing Pinger;

        public event EventHandler<ClientSocketEventArgs> Connected;
        public event EventHandler<ClientDisconnectedEventArgs> Disconnected;
        public event EventHandler<ClientSocketEventArgs> ResponsesLoaded;
        public event EventHandler<ClientSocketEventArgs> PingReceived;
        public event EventHandler<PacketEventArgs> PacketReading;
        public event EventHandler<PacketEventArgs> PacketReceived;
        public event EventHandler<PacketEventArgs> PacketSending;
        public event EventHandler<PacketEventArgs> PacketSent;
        public event EventHandler<RequestEventArgs> RequestSending;
        public event EventHandler<RequestEventArgs> RequestSent;
        public event EventHandler<ResponseEventArgs> ResponseLoaded;
        public event EventHandler<ResponseEventArgs> ResponseReading;
        public event EventHandler<ResponseEventArgs> ResponseRead;
        public event EventHandler<ClientExceptionEventArgs> ErrorThrowed;

        public NetworkStream Stream { get; private set; }
        public IPEndPoint EndPoint { get; set; }
        public IResponse[] Responses { get { return ResponseDict.Values.ToArray(); } }
        public int Interval { get { return SocketConstants.ReadQueueInterval; } }
        public int Ping { get { return Pinger.Ping; } }
        public IServerSocket Server { get; private set; }
        public bool IOEnabled { get; set; }
        public bool IsConnected { get; private set; }
        public Exception LastError { get; private set; }

        public ClientSocket()
        {
            ResponseDict = new Dictionary<uint, IResponse>();            
            IsConnected = false;

            IOQueue = new ClientIOQueue(this);
            Pinger = new ClientPing(this);
        }

        public ClientSocket(string DNS, int Port) : this()
        {
            ParseDNS(DNS, Port);
        }

        public ClientSocket(IPAddress Address, int Port) : this(new IPEndPoint(Address, Port))
        {

        }

        public ClientSocket(IPEndPoint EndPoint) : this()
        {
            this.EndPoint = EndPoint;
        }

        public ClientSocket(IServerSocket Server, Socket Client) : this()
        {
            this.Client = Client;
            this.Server = Server;
            this.EndPoint = (IPEndPoint)Client.RemoteEndPoint;

            Client.SendTimeout = SocketConstants.SendTimeout;
            Client.ReceiveTimeout = SocketConstants.ReceiveTimeout;            
        }

        public void ParseDNS(string DNS)
        {
            var Separator = DNS.IndexOf(':');
            int Port = -1;

            if (Separator == -1)
                throw new InvalidOperationException("Invalid DNS/Without port");
            else if (!int.TryParse(DNS.Substring(Separator + 1), out Port))
                throw new InvalidOperationException("Invalid port");
            else
                ParseDNS(DNS.Substring(0, Separator), Port);
        }

        public void ParseDNS(string DNS, int Port)
        {
            IPAddress Address;
            if (!IPAddress.TryParse(DNS, out Address))
            {
                var Addresses = Dns.GetHostAddresses(DNS);
                if (Addresses == null || Addresses.Length == 0 || !Addresses.Any(A => A.AddressFamily == AddressFamily.InterNetwork))
                    throw new InvalidOperationException("Invalid address " + DNS);
                else
                    Address = Addresses.First(A => A.AddressFamily == AddressFamily.InterNetwork);
            }

            EndPoint = new IPEndPoint(Address, Port);
        }

        public bool Connect()
        {
            try
            {
                Client = new Socket(EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                var Async = Client.BeginConnect(EndPoint, null, null);
                if (Async.AsyncWaitHandle.WaitOne(SocketConstants.ConnectTimeout))
                {
                    if (Server == null)
                        EventHelper.FireEvent(Connected, this, new ClientSocketEventArgs(this));

                    Initalize();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorCaught(ex, true);

                return false;
            }
        }

        internal void Initalize()
        {
            Client.NoDelay = true;
            Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            Client.LingerState = new LingerOption(true, SocketConstants.LingerSeconds);
            Client.ReceiveBufferSize = SocketConstants.ReceiveBufferSize;
            Client.ReceiveTimeout = SocketConstants.ReceiveTimeout;
            Client.SendBufferSize = SocketConstants.SendBufferSize;
            Client.SendTimeout = SocketConstants.SendTimeout;
            Client.Ttl = SocketConstants.Ttl;

            Stream = new NetworkStream(Client);

            IsConnected = true;

            UpdaterFactory.Start(IOQueue);
            UpdaterFactory.Start(Pinger);
            UpdaterFactory.Start(this);
        }

        public bool Disconnect()
        {
            try
            {
                Close(DisconnectReason.Normal);
                return true;
            }
            catch (Exception ex)
            {
                ErrorCaught(ex, true);

                return false;
            }
        }

        public bool Send(ISocketPacket Packet)
        {
            try
            {
                FirePacketSending(Packet);
                IOQueue.EnqueueWrite(Packet);

                return true;
            }
            catch (Exception ex)
            {
                ErrorCaught(ex, true);

                return false;
            }
        }

        public bool Send(IRequest Request)
        {
            EventHelper.FireEvent(RequestSending, this, new RequestEventArgs(this, Request));

            var Packet = new BitPacket(Request.ID);
            if (Request.Write(this, Packet) && Send(Packet))
            {
                EventHelper.FireEvent(RequestSent, this, new RequestEventArgs(this, Request));
                return true;
            }
            return false;
        }

        public void FirePacketSending(ISocketPacket Packet)
        {
            EventHelper.FireEvent(PacketSending, this, new PacketEventArgs(this, Packet));
        }

        public void FirePacketSent(ISocketPacket Packet)
        {
            EventHelper.FireEvent(PacketSent, this, new PacketEventArgs(this, Packet));
        }

        public void FirePingReceived()
        {
            EventHelper.FireEvent(PingReceived, this, new ClientSocketEventArgs(this));
        }

        public void FirePacketReceived(ISocketPacket Packet)
        {
            EventHelper.FireEvent(PacketReading, this, new PacketEventArgs(this, Packet));

            lock (responseLock)
            {
                IResponse Response = null;
                if (ResponseDict.TryGetValue(Packet.ID, out Response))
                {
                    var Args = new ResponseEventArgs(this, Response, Packet);
                    EventHelper.FireEvent(ResponseReading, this, Args);

#if !UNITY_5
                    if (ResponseReading != null)
                        EventHelper.FireEvent(ResponseReading, this, new ResponseEventArgs(this, Response, Packet));

                        if (Response.Read(Packet))
                        {
                            Response.Execute(this);
                            EventHelper.FireEvent(ResponseRead, this, new ResponseEventArgs(this, Response, Packet));
                        }
                        else
                            LoggerFactory.GetLogger(this).LogWarning("Failed to execute response of packet {0:X}!", Packet.ID);
#else
                    Local.Helper.SafeNetworkInvoker.Invoke(Args);
#endif
                }
            }

            EventHelper.FireEvent(PacketReceived, this, new PacketEventArgs(this, Packet));
        }

        public bool RegisterResponse<TResponse>() where TResponse : IResponse
        {
            return RegisterResponse<TResponse>(Assembly.GetCallingAssembly());
        }

        public bool RegisterResponse<TResponse>(params Assembly[] Assemblies) where TResponse : IResponse
        {
            try
            {
                lock (responseLock)
                {
                    if (ResponseDict != null)
                        ResponseDict.Clear();
                    else
                        ResponseDict = new Dictionary<uint, IResponse>();

                    foreach (var T in Assemblies.SelectMany(A => A.GetTypes().Where(T => !T.IsAbstract && !T.IsInterface && typeof(TResponse).IsAssignableFrom(T))))
                    {
                        var Response = (IResponse)Activator.CreateInstance(T);
                        EventHelper.FireEvent(ResponseLoaded, this, new ResponseEventArgs(this, Response, null));

                        ResponseDict.Add(Response.ID, Response);
                    }
                }
                EventHelper.FireEvent(ResponsesLoaded, this, new ClientSocketEventArgs(this));
                return true;
            }
            catch (Exception ex)
            {
                ErrorCaught(ex, true);

                return false;
            }
        }

        void IUpdater.Start()
        {

        }

        void IUpdater.Loop()
        {
            try
            {
                /*int Flag = Stream.ReadByte();
                if (Flag != -1)
                    Flag = SocketConstants.ChunkFlag - Flag;
                
                switch (Flag)
                {
                    case -1:
                        Close(DisconnectReason.EndOfStream);
                        break;
                    case SocketConstants.DisconnectFlag:
                        var Reason = (DisconnectReason)Stream.ReadByte();
                        Close(Reason);
                        break;
                    case SocketConstants.PingFlag:
                        Pinger.ReceivedPing();
                        break;
                    case SocketConstants.PacketFlag:
                        ReadPacket();
                        break;
                    default:
                        Close(DisconnectReason.Unknown);
                        break;
                }*/
                ReadPacket();
            }
            catch (Exception ex)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.Log(string.Format("Exception: {0}", ex.Message));
                UnityEngine.Debug.Log(ex.StackTrace);
#endif
                ErrorCaught(ex, true);
            }
        }

        void ReadPacket()
        {
            try
            {
                /*byte[] SizeBuffer = new byte[sizeof(int)];
                int Flag = -1, Compressed = -1;

                if ((Flag = Stream.ReadByte()) != -1 && (Compressed = Stream.ReadByte()) != -1 && ReadBuffer(SizeBuffer))
                {
                    SizeBuffer = SizeBuffer.Select(IOQueue.BufferTransform).ToArray();
                    byte[] Buffer = new byte[BitConverter.ToInt32(SizeBuffer, 0)];

                    if (ReadBuffer(Buffer))
                    {
                        //if (Compressed == 1)
                          //  Buffer = CompressionHelper.GZipDecompress(Buffer);
                        Buffer = RijndaelHelper.Decrypt(Buffer, Convert.ToByte(SocketConstants.ChunkFlag - Flag));

                        var Packet = new BitPacket(BitConverter.ToUInt16(Buffer, 0));
                        if (Buffer.Length > sizeof(uint))
                            Packet.Buffer = Buffer.Skip(sizeof(uint)).ToArray();

                        IOQueue.EnqueueRead(Packet);
                    }
                    else
                        Close(DisconnectReason.EndOfStream);
                }
                else
                    Close(DisconnectReason.EndOfStream);*/

                byte[] Header = new byte[sizeof(uint) * 2 + 2];
                if (!ReadBuffer(Header) || Header[0] != SocketConstants.HandshakeFlag)
                {

#if UNITY_EDITOR
                    UnityEngine.Debug.LogWarningFormat("H: {0:X} != {1:X}", Header[0], SocketConstants.HandshakeFlag);
#endif
                    Close(DisconnectReason.EndOfStream);
                }
                else if (Header[1] == SocketConstants.PingFlag)
                    Pinger.ReceivedPing();
                else if (Header[1] == SocketConstants.DisconnectFlag)
                {
                    var Reason = (DisconnectReason)Header[2];
                    Close(Reason);
                }
                else if (Header[1] == SocketConstants.PacketFlag)
                {
                    var Packet = new BitPacket(BitConverter.ToUInt32(Header, 2));
                    var Buffer = new byte[BitConverter.ToInt32(Header, 6)];

                    if (Buffer.Length > 0 && !ReadBuffer(Buffer))
                        Close(DisconnectReason.EndOfStream);
                    else
                    {
                        Packet.Buffer = Buffer;
                        IOQueue.EnqueueRead(Packet);
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorCaught(ex, true);
            }
        }

        private bool ReadBuffer(byte[] Buffer)
        {
            /*if (!(Client.Poll(SocketConstants.ReadQueueInterval, SelectMode.SelectRead) && Client.Available == 0))
            {
                int Total = 0;
                while (Total < Buffer.Length)
                {
                    if(!Stream.DataAvailable) { Thread.Sleep(10); continue; }
                    int Received = Stream.Read(Buffer, Total, Buffer.Length - Total);

                    if (Received == 0)
                        return false;
                    else
                        Total += Received;
                }

                if(Total > 0 && Buffer.Length == Total)
                {
                    return true;
                }
            }
            return false;*/

            try
            {
                int Received = Stream.Read(Buffer, 0, Buffer.Length);
                if (Received == 0)
                    Close(DisconnectReason.EndOfStream);

                if (Buffer.Length > Received)
                    Buffer = Buffer.Take(Received).ToArray();
                
                return Received > 0;
            }
            catch (Exception ex)
            {
                ErrorCaught(ex, true);
                LoggerFactory.GetLogger(this).LogFatal(ex);
                return false;
            }
        }

        void IUpdater.End()
        {
            Close(DisconnectReason.Normal);
        }

        internal void Close(DisconnectReason Reason)
        {
            lock (syncLock)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.Log("Reason: {0}".Format(Reason));
                UnityEngine.Debug.Log(UnityEngine.StackTraceUtility.ExtractStackTrace());
#endif
                if (IsConnected)
                {
                    IsConnected = false;
                    IOEnabled = false;

                    try
                    {
                        UpdaterFactory.Stop(this);
                        UpdaterFactory.Stop(Pinger);
                        UpdaterFactory.Stop(IOQueue);

                        if (Stream.CanWrite && Server == null)
                        {
                            var B = new byte[3];
                            B[0] = SocketConstants.HandshakeFlag;
                            B[1] = SocketConstants.DisconnectFlag;
                            B[2] = (byte)Reason;
                            Stream.Write(B, 0, B.Length);
                        }

                        if (Client.Connected)
                            Client.Shutdown(SocketShutdown.Both);

                        Stream.Close();
                        Client.Close();
                    }
                    catch (Exception ex)
                    {
                        ErrorCaught(ex, false);
                        Reason = DisconnectReason.Error;
                    }
                    EventHelper.FireEvent(Disconnected, this, new ClientDisconnectedEventArgs(this, Reason));

                    if (Server != null)
                        Server.FireClientDisconnected(this);
                }
            }
        }

        internal void ErrorCaught(Exception ex, bool Close)
        {
            LastError = ex;
            /*if (!SocketConstants.SkipException(ex))
            {*/
                EventHelper.FireEvent(ErrorThrowed, this, new ClientExceptionEventArgs(this, ex));
                if (Close)
                    this.Close(DisconnectReason.Error);
            /*}
            else if (Close)
                this.Close(DisconnectReason.EndOfStream);*/
        }

        public bool Equals(IClientSocket Socket)
        {
            return Socket.EndPoint.Address.ToString() == EndPoint.Address.ToString() && Socket.EndPoint.Port == EndPoint.Port;
        }
    }
}