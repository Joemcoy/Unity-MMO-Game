using System;
using System.Net;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

using tFramework.Interfaces;
namespace tFramework.Network.Bases
{
    using Interfaces;
    using EventArgs;
    using Helper;
    using Enums;
    
    public abstract class BaseServer<TServer, TNetworkServer, TClient, TNetworkClient> : IComponent
        where TServer : BaseServer<TServer, TNetworkServer, TClient, TNetworkClient>, new()
        where TNetworkServer : INetworkServer<TNetworkServer, TNetworkClient>, new()
        where TClient : BaseClient<TClient, TNetworkClient>
        where TNetworkClient : INetworkClient<TNetworkClient>, new()
    {
        private List<TClient> clientList;
        private Dictionary<ushort, IResponse<TNetworkClient>> responses;

        public event EventHandler<BaseServerEventArgs<TServer, TNetworkServer, TClient, TNetworkClient>> OnOpened, OnClosed;
        public event EventHandler<BaseClientEventArgs<TClient, TNetworkClient>> OnConnected, OnDisconnected;

        public TNetworkServer Socket { get; private set; }
        public TClient[] Clients { get { return clientList.ToArray(); } }

        public BaseServer() : this(0) { }
        public BaseServer(int port) : this(IPAddress.Any, port) { }
        public BaseServer(IPAddress ip, int port) : this(new IPEndPoint(ip, port)) { }
        public BaseServer(IPEndPoint endPoint) : this(new TNetworkServer()) { Socket.EndPoint = endPoint; }
        private BaseServer(TNetworkServer server)
        {
            clientList = new List<TClient>();
            responses = new Dictionary<ushort, IResponse<TNetworkClient>>();

            Socket = server;
            Socket.OnOpen += Server_OnOpen;
            Socket.OnClose += Server_OnClose;
            Socket.OnConnected += Server_OnConnected;
        }

        protected virtual TClient CreateClientInstance(TNetworkClient client)
        {
            var tc = Activator.CreateInstance<TClient>();
            tc.LoadEvents(client);

            return tc;
        }

        bool IComponent.Enable() { return Enable(); }
        protected virtual bool Enable() { return Socket.Open(); }

        bool IComponent.Disable() { return Disable(); }
        protected virtual bool Disable() { return Socket.Close(); }

        protected virtual void Opened() { }
        private void Server_OnOpen(object sender, ServerEventArgs<TNetworkServer, TNetworkClient> e)
        {
            Opened();
            OnOpened.FireEvent(new BaseServerEventArgs<TServer, TNetworkServer, TClient, TNetworkClient>((TServer)this), this);
        }

        public void RegisterResponses(params Assembly[] assemblies)
        {
            if (assemblies.Length == 0)
                assemblies = new Assembly[] { Assembly.GetCallingAssembly() };
            RegisterResponses<BaseResponse<TClient, TNetworkClient>>(assemblies);
        }

        public void RegisterResponses<TResponse>(params Assembly[] assemblies)
            where TResponse : IResponse<TNetworkClient>
        {
            if (assemblies.Length == 0)
                assemblies = new Assembly[] { Assembly.GetCallingAssembly() };

            var responseType = typeof(TResponse);
            foreach (Assembly target in assemblies)
            {
                foreach (Type foundType in target.GetTypes())
                {
                    if (!foundType.IsAbstract && !foundType.IsInterface && responseType.IsAssignableFrom(foundType))
                    {
                        var response = (IResponse<TNetworkClient>)Activator.CreateInstance(foundType);
                        responses[response.ID] = response;
                    }
                }
            }
        }

        protected virtual void Closed() { }
        private void Server_OnClose(object sender, ServerEventArgs<TNetworkServer, TNetworkClient> e)
        {
            Closed();
            OnClosed.FireEvent(new BaseServerEventArgs<TServer, TNetworkServer, TClient, TNetworkClient>((TServer)this), this);
        }

        protected virtual void Connected(TClient client) { }
        private void Server_OnConnected(object sender, ClientEventArgs<TNetworkClient> e)
        {
            e.Client.OnDisconnect += Client_OnDisconnect;
            var client = CreateClientInstance(e.Client);
            //client.Socket = e.Client;

            if(responses.Count > 0)
            {
                foreach(var response in responses.Values.OfType<BaseResponse<TClient, TNetworkClient>>())
                {
                    var clone = response.Clone();
                    clone.Client = client;
                    client.LoadResponse(clone);
                    e.Client.RegisterResponse(clone);
                }
            }

            clientList.Add(client);
            Connected(client);
            OnConnected.FireEvent(new BaseClientEventArgs<TClient, TNetworkClient>(client), this);
        }

        protected virtual void Disconnected(TClient client, DisconnectReason reason) { }
        private void Client_OnDisconnect(object sender, DisconnectedEventArgs<TNetworkClient> e)
        {
            var client = clientList.FirstOrDefault(c => c.Socket.Equals(e.Client));
            if (client != null)
            {
                Disconnected(client, e.Reason);
                OnDisconnected.FireEvent(new BaseClientEventArgs<TClient, TNetworkClient>(client as TClient), this);
                clientList.Remove(client);
            }
        }

        public void SendToAll(IRequest<TNetworkClient> request, Predicate<TClient> predicate = null)
        {
            if (clientList.Count > 0)
            {
                var packet = clientList[0].Socket.CreatePacket(request.ID);
                foreach (var client in clientList)
                {
                    if (predicate == null || predicate(client))
                    {
                        request.Write(client.Socket, packet);
                        client.Socket.Send(packet);
                        packet.Clear();
                    }
                }
            }
        }

        public void SendToAll(IDataPacket packet, Predicate<TClient> predicate = null)
        {
            foreach (var client in clientList)
                if (predicate == null || predicate(client))
                    client.Socket.Send(packet);
        }
    }
}