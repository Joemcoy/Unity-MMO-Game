using System;
using System.Collections.Generic;
using System.Net;

using Launcher.Client;
using Base.Data.Interfaces;
using Base.Factories;

using Server.Configuration;

using Network.Data.Interfaces;
using Network.v1;
using Data.Client;
using Game.Data.Models;

namespace Launcher.Server
{
    public class LauncherServer : ISingleton, IComponent, IServerSocketDispatcher
    {
        public ServerSocket Server;
        Dictionary<IPEndPoint, LauncherClient> ClientDict = null;

        public static LauncherClient[] Clients
        {
            get
            {
                LauncherServer Server = SingletonFactory.GetInstance<LauncherServer>();

                LauncherClient[] _Clients = new LauncherClient[Server.ClientDict.Count];
                Server.ClientDict.Values.CopyTo(_Clients, 0);

                return _Clients;
            }
        }

        public LauncherFileModel[] Files { get; set; }

        #region ISingleton implementation
        public void Create()
        {
            ClientDict = new Dictionary<IPEndPoint, LauncherClient>();
            Server = new ServerSocket(2015);
            Server.RegisterDispatcher(this);
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
                    DataClient Data = SingletonFactory.GetInstance<DataClient>();
                    Data.Socket.RegisterResponse<DCReader>();

                    if (!ComponentFactory.Enable<GatesConfiguration>())
                        return false;
                    else if (!ComponentFactory.Enable<DataClient>())
                        return false;
                    else
                    {
                        Server.EndPoint.Port = PortsConfiguration.LauncherPort;
                        Data.SendLauncherFilesRequest(this);

                        LoggerFactory.GetLogger().LogWarning("Waiting for launcher file list...");
                        return Server.Open();
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerFactory.GetLogger().LogFatal(ex);
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
                LoggerFactory.GetLogger().LogFatal(ex);
                return false;
            }
        }
        #endregion

        public void OnOpen(IServerSocket Server)
        {
            LoggerFactory.GetLogger().LogInfo("Launcher server has been opened!");
        }

        public void OnClose(IServerSocket Server)
        {
            LoggerFactory.GetLogger().LogInfo("Launcher server has been closed!");
        }

        public void OnClientConnect(IClientSocket Client)
        {
            LoggerFactory.GetLogger().LogInfo($"Client <{Client.EndPoint}> has been connected!");

            LauncherClient Launcher = new LauncherClient(Client);
            ClientDict.Add(Client.EndPoint, Launcher);

            DataClient Data = SingletonFactory.GetInstance<DataClient>();
            Data.SendMessages(Launcher);
        }

        public void OnClientDisconnect(IClientSocket Client)
        {
            if (ClientDict.ContainsKey(Client.EndPoint))
                ClientDict.Remove(Client.EndPoint);
        }

        public void OnError(IClientSocket Client, Exception Error)
        {
            LoggerFactory.GetLogger().LogError("Server has caught a exception!");
            LoggerFactory.GetLogger().LogFatal(Error);
        }

        public void OnDispatch(Action Method)
        {
            Method();
        }
    }
}