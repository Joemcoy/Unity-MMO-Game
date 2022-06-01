#if !UNITY_5
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Base.Factories;
using Network.Data.EventArgs;
using Network.Data.Interfaces;
using Network.v1;
using Network.ArgumentReceivers;

namespace Network.Bases
{
    public abstract class ServerBase<TClient>
        where TClient : ClientBase<TClient>, new()
    {
        private List<TClient> ClientList;

        public IServerSocket Socket { get; private set; }
        public TClient[] Clients { get { return ClientList.ToArray(); } }
        protected virtual Assembly ResponsesAssembly { get; }

        public ServerBase()
        {
            ClientList = new List<TClient>();
            Socket = new ServerSocket();
            Socket.Opened += Server_Opened;
            Socket.Closed += Server_Closed;
            Socket.ClientConnected += Server_ClientConnected;
            Socket.ErrorThrowed += Server_ErrorThrowed;
        }

        private void Server_Opened(object sender, ServerSocketEventArgs e)
        {
            if (SocketArguments.DebugSocket)
                LoggerFactory.GetLogger(this).LogInfo("Data server has been opened!");
        }

        private void Server_Closed(object sender, ServerSocketEventArgs e)
        {
            if (SocketArguments.DebugSocket)
                LoggerFactory.GetLogger(this).LogInfo("Data server has been closed!");
        }

        private void Server_ClientConnected(object sender, ClientConnectedEventArgs e)
        {
            if (SocketArguments.DebugSocket)
                LoggerFactory.GetLogger(this).LogInfo("Client <{0}> has been connected!", e.Client.EndPoint);

            e.Client.Disconnected += Client_Disconnected;

            var Client = (TClient)Activator.CreateInstance(typeof(TClient), false);
            Client.Socket = e.Client;
            Client.Server = this;
            Client.LoadEvents();

            ClientList.Add(Client);
            e.Client.RegisterResponse<ResponseBase<TClient>>(ResponsesAssembly);
        }

        private void Client_Disconnected(object sender, ClientDisconnectedEventArgs e)
        {
            ClientList.Remove(ClientList.First(C => C.Socket.Equals(e.Client)));
        }

        private void Server_ErrorThrowed(object sender, ServerExceptionEventArgs e)
        {
            if (SocketArguments.DebugSocket)
                LoggerFactory.GetLogger(this).LogInfo("Server has caught a exception!");
            LoggerFactory.GetLogger(this).LogFatal(e.Error);
        }
    }
}
#endif