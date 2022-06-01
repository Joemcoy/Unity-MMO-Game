using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Base.Data.Interfaces;
using Base.Factories;

using Network.Data;
using Network.Data.Interfaces;
using System.Diagnostics;
using Network.Data.Enums;

namespace Network.v1
{
    public class ClientPing : IUpdater
    {
        private ClientSocket Socket;
        private DateTime Last;
        private bool CanPing = false;

        public int Interval { get { return SocketConstants.PingInterval; } }
        public int Ping { get; private set; }

        public ClientPing(ClientSocket Socket)
        {
            this.Socket = Socket;
            Ping = -1;
        }

        public void Start()
        {
            CanPing = true;
            //LoggerFactory.GetLogger(this).LogInfo("Pinger started for endpoint {0}!", Socket.EndPoint);
        }       

        public void End()
        {
            Ping = -1;

            if (Socket.IsConnected)
                Socket.Close(DisconnectReason.EndOfStream);
        }

        public void ReceivedPing()
        {
            Ping = (int)Math.Round((DateTime.Now - Last).TotalMilliseconds);
            Socket.FirePingReceived();

            CanPing = true;
        }

        public void Loop()
        {
            try
            {
                if (CanPing)
                {
                    Last = DateTime.Now;
                    CanPing = false;

                    var B = new byte[2];
                    B[0] = SocketConstants.HandshakeFlag;
                    B[1] = SocketConstants.PingFlag;
                    Socket.Stream.Write(B, 0, B.Length);
                    //Socket.Stream.Write(BitConverter.GetBytes(SocketConstants.ChunkFlag - SocketConstants.PingFlag), 0, sizeof(byte));
                }
            }
            catch(Exception ex)
            {
                Socket.ErrorCaught(ex, true);
            }
        }
    }
}
