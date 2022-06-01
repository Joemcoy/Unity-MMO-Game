using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base.Factories;
using Network.Data.EventArgs;
using Network.Data.Interfaces;
using Network.v1;
using Network.ArgumentReceivers;

namespace Network.Bases
{
    public abstract class ClientBase<TClient>
        where TClient : ClientBase<TClient>, new()
    {
        public IClientSocket Socket { get; internal set; }

#if !UNITY_5
        public ServerBase<TClient> Server { get; internal set; }
#endif

        public ClientBase(bool LoadSocket = true)
        {
            if (LoadSocket)
            {
                Socket = new ClientSocket();
                LoadEvents();
            }
        }

        internal void LoadEvents()
        {
            Socket.Connected += Socket_Connected;
            Socket.Disconnected += Socket_Disconnected;
            Socket.PacketReading += Socket_PacketReading;
            Socket.PacketReceived += Socket_PacketReceived;
            Socket.PacketSending += Socket_PacketSending;
            Socket.PacketSent += Socket_PacketSent;
            Socket.RequestSending += Socket_RequestSending;
            Socket.RequestSent += Socket_RequestSent;
            Socket.ResponseReading += Socket_ResponseReading;
            Socket.ResponseRead += Socket_ResponseRead;
            Socket.ResponseLoaded += Socket_ResponseLoaded;
            Socket.ResponsesLoaded += Socket_ResponsesLoaded;
            Socket.ErrorThrowed += Socket_ErrorThrowed;
        }

        internal ClientBase(IClientSocket Socket) : base()
        {
            this.Socket = Socket;
        }


        private void Socket_Connected(object sender, ClientSocketEventArgs e)
        {
#if UNITY_5
            Local.Scripts.SocketRegister.RegisterSocket(e.Client);
#endif
            if(SocketArguments.DebugSocket)
            LoggerFactory.GetLogger(this).LogInfo("Client <{0}> has been connected!", e.Client.EndPoint);
            Socket.RegisterResponse<ResponseBase<TClient>>();
        }

        private void Socket_Disconnected(object sender, ClientDisconnectedEventArgs e)
        {
#if UNITY_5
            Local.Scripts.SocketRegister.RemoveSocket(e.Client);
#endif
            if (SocketArguments.DebugSocket)
            LoggerFactory.GetLogger(this).LogInfo("Client <{0}> has been disconnected by: {1}!", Socket.EndPoint, e.Reason);
        }

        private void Socket_PacketReading(object sender, PacketEventArgs e)
        {
            //LoggerFactory.GetLogger(this).LogInfo("Reading packet {0:X} from {1}!", e.Packet.ID.ToString("X4"), Socket.EndPoint);
        }

        private void Socket_PacketReceived(object sender, PacketEventArgs e)
        {
            //LoggerFactory.GetLogger(this).LogInfo("Packet {0:X} received from {1}!", e.Packet.ID.ToString("X4"), Socket.EndPoint);
        }

        private void Socket_PacketSending(object sender, PacketEventArgs e)
        {
            //LoggerFactory.GetLogger(this).LogInfo("Sending packet {0:X} to {1}!", e.Packet.ID.ToString("X4"), Socket.EndPoint);
        }

        private void Socket_PacketSent(object sender, PacketEventArgs e)
        {
            //LoggerFactory.GetLogger(this).LogInfo("Packet {0:X} has been sent to {1}!", e.Packet.ID.ToString("X4"), Socket.EndPoint);
        }

        private void Socket_RequestSending(object sender, RequestEventArgs e)
        {
            //LoggerFactory.GetLogger(this).LogInfo("Sending a request of packet {0:X} to {1}!", e.Request.ID, Socket.EndPoint);
        }

        private void Socket_RequestSent(object sender, RequestEventArgs e)
        {
            //LoggerFactory.GetLogger(this).LogInfo("Packet {0:X} as been sent to {1}!", e.Request.ID, Socket.EndPoint);
        }

        private void Socket_ResponseReading(object sender, ResponseEventArgs e)
        {
            //LoggerFactory.GetLogger(this).LogInfo("Response of packet {0:X} received from {1}!", e.Response.ID, Socket.EndPoint);
        }

        private void Socket_ResponseRead(object sender, ResponseEventArgs e)
        {
            //LoggerFactory.GetLogger(this).LogInfo("Response of packet {0:X} as been readed!", e.Response.ID, Socket.EndPoint);
        }

        private void Socket_ResponseLoaded(object sender, ResponseEventArgs e)
        {
            (e.Response as ResponseBase<TClient>).Client = (TClient)this;
        }

        private void Socket_ResponsesLoaded(object sender, ClientSocketEventArgs e)
        {
            e.Client.IOEnabled = true;
        }

        private void Socket_ErrorThrowed(object sender, ClientExceptionEventArgs e)
        {
            //if (SocketArguments.DebugSocket)
                LoggerFactory.GetLogger(this).LogFatal(e.Error);
        }
    }
}