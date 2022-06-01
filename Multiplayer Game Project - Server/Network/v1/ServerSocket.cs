#if !UNITY_5
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;

using Base.Data.Interfaces;
using Base.Data.Abstracts;
using Base.Factories;

using Network.Data;
using Network.Data.Interfaces;
using Network.Data.Dispatchers;
using Network.Data.EventArgs;
using Base.Helpers;
using Network.Data.Enums;

namespace Network.v1
{
    public class ServerSocket : IServerSocket, IUpdater
    {
        Socket Server;
        List<IClientSocket> ClientList;

        public event EventHandler<ServerSocketEventArgs> Opened;
        public event EventHandler<ServerSocketEventArgs> Closed;
        public event EventHandler<ClientConnectedEventArgs> ClientConnected;
        public event EventHandler<ServerExceptionEventArgs> ErrorThrowed;

        int IUpdater.Interval
        {
            get
            {
                return SocketConstants.AcceptInterval;
            }
        }

        public IPEndPoint EndPoint { get; set; }
        public IClientSocket[] Clients { get { return ClientList.ToArray(); } }
        public int MaximumConnections { get; set; }
        public bool IsOpened { get; private set; }

        public ServerSocket() :this(IPAddress.Any, 0) { }
        public ServerSocket(int Port) : this(IPAddress.Any, Port) { }
        public ServerSocket(IPAddress IP, int Port) : this(new IPEndPoint(IP, Port)) { }
        public ServerSocket(IPEndPoint EndPoint)
        {
            this.EndPoint = EndPoint;
            ClientList = new List<IClientSocket>();

            MaximumConnections = SocketConstants.MaximumConnections;
            IsOpened = false;
        }

        public bool Open()
        {
            try
            {
                Server = new Socket(EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                Server.Bind(EndPoint);
                Server.Listen(1);

                EventHelper.FireEvent(Opened, this, new ServerSocketEventArgs(this));
                UpdaterFactory.Start(this);

                IsOpened = true;
                return true;
            }
            catch(Exception ex)
            {
                EventHelper.FireEvent(ErrorThrowed, this, new ServerExceptionEventArgs(this, ex));
                return false;
            }
        }

        public bool Close()
        {
            try
            {
                if(UpdaterFactory.HasStarted(this))
                    UpdaterFactory.Stop(this);
                DisconnectAll();

                Server.Close();
            }
            catch (Exception ex)
            {
                if (!SocketConstants.SkipException(ex))
                {
                    EventHelper.FireEvent(ErrorThrowed, this, new ServerExceptionEventArgs(this, ex));
                    return false;
                }
            }
            finally
            {
                EventHelper.FireEvent(Closed, this, new ServerSocketEventArgs(this));
                IsOpened = false;
            }
            return true;
        }

        public bool SendToAll(IRequest Request)
        {
            try
            {
                foreach (var Client in ClientList)
                    if (!Client.Send(Request))
                        return false;

                return true;
            }
            catch (Exception ex)
            {
                if (!SocketConstants.SkipException(ex))
                {
                    EventHelper.FireEvent(ErrorThrowed, this, new ServerExceptionEventArgs(this, ex));
                }
                return false;
            }
        }

        public bool SendToAll(ISocketPacket Packet)
        {
            try
            {
                foreach (var Client in ClientList)
                    if (!Client.Send(Packet))
                        return false;

                return true;
            }
            catch (Exception ex)
            {
                if (!SocketConstants.SkipException(ex))
                {
                    EventHelper.FireEvent(ErrorThrowed, this, new ServerExceptionEventArgs(this, ex));
                }
                return false;
            }
        }

        public bool DisconnectAll()
        {
            try
            {
                var Temp = new IClientSocket[ClientList.Count];
                ClientList.CopyTo(Temp);

                foreach (var Client in Temp)
                    if (!Client.Disconnect())
                        return false;

                ClientList.Clear();
                return true;
            }
            catch (Exception ex)
            {
                if (!SocketConstants.SkipException(ex))
                {
                    EventHelper.FireEvent(ErrorThrowed, this, new ServerExceptionEventArgs(this, ex));
                }
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
                var Socket = Server.Accept();
                var Client = new ClientSocket(this, Socket);

                if (ClientList.Count == SocketConstants.MaximumConnections)
                    Client.Close(DisconnectReason.MaximumReached);
                else
                {
                    ClientList.Add(Client);
                    EventHelper.FireEvent(ClientConnected, this, new ClientConnectedEventArgs(this, Client));

                    Client.Initalize();
                }
            }
            catch (Exception ex)
            {
                if (!SocketConstants.SkipException(ex))
                {
                    EventHelper.FireEvent(ErrorThrowed, this, new ServerExceptionEventArgs(this, ex));
                    Close();
                }
            }
        }

        public void End()
        {
            Close();
        }

        public void FireClientDisconnected(IClientSocket Client)
        {
            if (ClientList.Contains(Client))
            {
                ClientList.Remove(Client);
            }
        }
    }
}
#endif