using System;
using System.Collections.Generic;
using System.Net;

using Chat.Client;
using Base.Data.Interfaces;
using Base.Factories;

using Server.Configuration;

using Network.Data.Interfaces;
using Network.v1;

using Gate.Client;
using Data.Client;
using Game.Data.Enums;
using Gate.Server;
using Gate.Client.Responses.Writers;
using Network.Data.Dispatchers;
using Network.Data.EventArgs;
using Base.Configurations;

namespace Chat.Server
{
    public class ChatServer : ISingleton, IComponent
    {
        public ServerSocket Server;
        Dictionary<IClientSocket, ChatClient> ClientDict = null;

        public static ChatClient[] Clients
        {
            get
            {
                ChatServer Server = SingletonFactory.GetInstance<ChatServer>();

                ChatClient[] _Clients = new ChatClient[Server.ClientDict.Count];
                Server.ClientDict.Values.CopyTo(_Clients, 0);

                return _Clients;
            }
        }

        #region ISingleton implementation
        public void Create()
        {
            ClientDict = new Dictionary<IClientSocket, ChatClient>();

            Server = new ServerSocket(0);
            Server.Opened += Server_Opened;
            Server.Closed += Server_Closed;
            Server.ClientConnected += Server_ClientConnected;
            Server.ErrorThrowed += Server_ErrorThrowed;
        }

        private void Server_Opened(object sender, ServerSocketEventArgs e)
        {
            LoggerFactory.GetLogger(this).LogInfo("Chat server has been opened!");
        }

        private void Server_Closed(object sender, ServerSocketEventArgs e)
        {
            LoggerFactory.GetLogger(this).LogInfo("Chat server has been closed!");
        }

        private void Server_ClientConnected(object sender, ClientConnectedEventArgs e)
        {
            LoggerFactory.GetLogger(this).LogInfo("Client <{0}> has been connected!", e.EndPoint);

            e.Client.Disconnected += Client_Disconnected;
            ChatClient Chat = new ChatClient(e.Client as ClientSocket);
            ClientDict.Add(e.Client, Chat);

            e.Client.RegisterResponse<CCResponse>();
        }

        private void Client_Disconnected(object sender, ClientDisconnectedEventArgs e)
        {
            if (!ClientDict.Remove(e.Client))
                LoggerFactory.GetLogger(this).LogWarning("Failed to remove client {0}!", e.EndPoint);
        }

        private void Server_ErrorThrowed(object sender, ServerExceptionEventArgs e)
        {
            LoggerFactory.GetLogger(this).LogInfo("Server has caught a exception!");
            LoggerFactory.GetLogger(this).LogFatal(e.Error);
        }

        public void Destroy()
        {

        }
        #endregion        

        #region IComponent implementation
        public bool Enable()
        {
            try
            {
                if (!ComponentFactory.Enable<PortsConfiguration>())
                    return false;
                else
                {
                    GateServer Gate = SingletonFactory.GetInstance<GateServer>();
                    Gate.Port = PortsConfiguration.ChatGatePort;
                    Gate.CurrentAssembly = typeof(ChatServer).Assembly;

                    GateClient Client = SingletonFactory.GetInstance<GateClient>();
                    Client.Type = GateType.Chat;
                    Client.Port = PortsConfiguration.ChatPort;
                    Client.Socket.RegisterResponse<GTResponse>();

                    DataClient Data = SingletonFactory.GetInstance<DataClient>();
                    Data.Socket.RegisterResponse<DCResponse>();
                    Data.Socket.CanReadPacket = true;

                    if (!ComponentFactory.Enable<IntervalConfiguration>())
                        return false;
                    else if (!ComponentFactory.Enable<GatesConfiguration>())
                        return false;
                    else if (!ComponentFactory.Enable<GateServer>())
                        return false;
                    else if (!ComponentFactory.Enable<GateClient>())
                        return false;
                    else if (!ComponentFactory.Enable<DataClient>())
                        return false;
                    else
                    {
                        SendGateTypeWriter Packet = new SendGateTypeWriter();
                        Packet.Type = GateType.Chat;
                        Packet.Gate = new Game.Data.Information.GateInfo() { Port = PortsConfiguration.ChatPort };
                        Client.Socket.Send(Packet);

                        Server.EndPoint.Port = PortsConfiguration.ChatPort;
                        return Server.Open();
                    }
                }
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
                Server.Close();
                return true;
            }
            catch (Exception ex)
            {
                LoggerFactory.GetLogger(this).LogFatal(ex);
                return false;
            }
        }
        #endregion
    }
}