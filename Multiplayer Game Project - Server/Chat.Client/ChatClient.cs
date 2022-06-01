using System;

using Network.Data;
using Network.Data.Interfaces;
using Network.v1;

using Base.Data.Enums;
using Game.Data;
using Game.Data.Interfaces;
using Base.Factories;
using Game.Data.Enums;
using Base.Data.Interfaces;
using Game.Data.Models;
using Network.Data.EventArgs;
using Base.Data.Abstracts;

namespace Chat.Client
{
    public class ChatClient
#if UNITY_BUILD || UNITY_STANDALONE || UNITY_EDITOR
        : ASingleton<ChatClient>, IComponent
#endif
    {
        public ClientSocket Socket { get; private set; }
        public AccountModel Account { get; set; }

        public ChatClient() : this(new ClientSocket())
        {
        }

        public ChatClient(ClientSocket Socket)
        {
            this.Socket = Socket;

            this.Socket.Connected += Socket_Connected;
            this.Socket.Disconnected += Socket_Disconnected;
            this.Socket.PacketReading += Socket_PacketReading;
            this.Socket.PacketReceived += Socket_PacketReceived;
            this.Socket.PacketSending += Socket_PacketSending;
            this.Socket.PacketSent += Socket_PacketSent;
            this.Socket.RequestSending += Socket_RequestSending;
            this.Socket.RequestSent += Socket_RequestSent;
            this.Socket.ResponseReading += Socket_ResponseReading;
            this.Socket.ResponseRead += Socket_ResponseRead;
            this.Socket.ResponseLoaded += Socket_ResponseLoaded;
            this.Socket.ResponsesLoaded += Socket_ResponsesLoaded;
            this.Socket.ErrorThrowed += Socket_ErrorThrowed;
        }

        private void Socket_ErrorThrowed(object sender, ClientExceptionEventArgs e)
        {
            LoggerFactory.GetLogger(this).LogFatal(e.Error);
        }

        private void Socket_Connected(object sender, ClientSocketEventArgs e)
        {
#if UNITY_5
            Local.Scripts.SocketRegister.RegisterSocket(Socket);
#endif

            LoggerFactory.GetLogger(this).LogInfo("Client <{0}> has been connected!", e.Client.EndPoint);
            Socket.RegisterResponse<CCResponse>();
        }

        private void Socket_Disconnected(object sender, ClientDisconnectedEventArgs e)
        {
            LoggerFactory.GetLogger(this).LogInfo("Client <{0}> has been disconnected by: {1}!", Socket.EndPoint, e.Reason);
#if UNITY_5
            Local.Scripts.UI.Interface.Windows.MessageBox.Show("Erro", "Você foi desconectado pelo servidor de contas...");
#endif
        }

        private void Socket_PacketReading(object sender, PacketEventArgs e)
        {
            //LoggerFactory.GetLogger(this).LogInfo("Reading packet {0} from {1}!", e.Packet.ID, Socket.EndPoint);
        }

        private void Socket_PacketReceived(object sender, PacketEventArgs e)
        {
            //LoggerFactory.GetLogger(this).LogInfo("Packet {0} received from {1}!", e.Packet.ID, Socket.EndPoint);
        }

        private void Socket_PacketSending(object sender, PacketEventArgs e)
        {
            //LoggerFactory.GetLogger(this).LogInfo("Sending packet {0} to {1}!", e.Packet.ID, Socket.EndPoint);
        }

        private void Socket_PacketSent(object sender, PacketEventArgs e)
        {
            //LoggerFactory.GetLogger(this).LogInfo("Packet {0} has been sent to {1}!", e.Packet.ID, Socket.EndPoint);
        }

        private void Socket_RequestSending(object sender, RequestEventArgs e)
        {
            //LoggerFactory.GetLogger(this).LogInfo("Sending a request of packet {0} to {1}!", e.Request.ID, Socket.EndPoint);
        }

        private void Socket_RequestSent(object sender, RequestEventArgs e)
        {
            //LoggerFactory.GetLogger(this).LogInfo("Sending of packet {0} as been sent to {1}!", e.Request.ID, Socket.EndPoint);
        }

        private void Socket_ResponseReading(object sender, ResponseEventArgs e)
        {
            //LoggerFactory.GetLogger(this).LogInfo("Response of packet {0} received from {1}!", e.Response.ID, Socket.EndPoint);
#if UNITY_BUILD || UNITY_STANDALONE || UNITY_EDITOR
            e.Cancel = true;
            e.Client.CanReadPacket = false;
            Local.Helper.SafeNetworkInvoker.Invoke(e);
#endif
        }

        private void Socket_ResponseRead(object sender, ResponseEventArgs e)
        {
            //LoggerFactory.GetLogger(this).LogInfo("Response of packet {0} as been readed!", e.Response.ID, Socket.EndPoint);
        }

        private void Socket_ResponseLoaded(object sender, ResponseEventArgs e)
        {
            (e.Response as CCResponse).Client = this;
        }

        private void Socket_ResponsesLoaded(object sender, ClientSocketEventArgs e)
        {
            e.Client.CanReadPacket = true;
        }

#if UNITY_5
        public override void Create()
        {

        }

        public override void Destroy()
        {
            Local.Scripts.SocketRegister.RemoveSocket(Socket);
        Socket.Disconnect();
        }
#endif

#if UNITY_BUILD || UNITY_STANDALONE || UNITY_EDITOR
        public bool Enable()
        {
            try
            {
                Socket.Connect();

                return true;
            }
            catch (Exception ex)
            {
                LoggerFactory.GetLogger(this).LogFatal(ex);
                return false;
            }
        }

        public bool Disable()
        {
            try
            {
                Socket.Disconnect();
                return true;
            }
            catch (Exception ex)
            {
                LoggerFactory.GetLogger(this).LogFatal(ex);
                return false;
            }
        }
#endif
    }
}