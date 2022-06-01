using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Reflection;

namespace tFramework.Network.Bases
{
    using Interfaces;
    using EventArgs;
    using Helper;
    using Enums;

    public abstract class BaseClient<TClient, TNetworkClient> : IBaseClient<TNetworkClient>, IEquatable<TClient>
        where TClient : BaseClient<TClient, TNetworkClient>
        where TNetworkClient : INetworkClient<TNetworkClient>, new()
    {
        public TNetworkClient Socket { get; internal set; }
        public event EventHandler<BaseClientEventArgs<TClient, TNetworkClient>> OnConnected;
        public event EventHandler<BaseDisconnectedEventArgs<TClient, TNetworkClient>> OnDisconnected;
        public event EventHandler<BaseClientErrorEventArgs<TClient, TNetworkClient>> OnError;

        public BaseClient(bool init = true)
        {
            if (init)
                LoadEvents(Activator.CreateInstance<TNetworkClient>());
        }
        public BaseClient(IPEndPoint endPoint) : this() { Socket.EndPoint = endPoint; }
        public BaseClient(IPAddress ip, int port) : this() { Socket.EndPoint = new IPEndPoint(ip, port); }

        protected internal void LoadEvents(TNetworkClient client)
        {
            if (client != null)
            {
                Socket = client;
                Socket.OnConnect += Client_OnConnect;
                Socket.OnDisconnect += Client_OnDisconnect;
                Socket.OnPacketWrite += Client_OnPacketWrite;
                Socket.OnPacketSent += Client_OnPacketSent;
                Socket.OnPacketRead += Client_OnPacketRead;
                Socket.OnPacketReceive += Client_OnPacketReceive;
                Socket.OnRequestWrite += Socket_OnRequestWrite;
                Socket.OnResponseExecute += Client_OnResponseExecute;
                Socket.OnError += Socket_OnError;

                Initalize();
            }
        }

        protected virtual void Initalize()
        {
            Socket.IOEnabled = true;
        }

        protected internal virtual void LoadResponse(BaseResponse<TClient, TNetworkClient> response) { }
        public void RegisterResponses(params Assembly[] assemblies)
        {
            RegisterResponses<BaseResponse<TClient, TNetworkClient>>(assemblies);
        }

        public void RegisterResponses<TResponse>(params Assembly[] assemblies)
            where TResponse : BaseResponse<TClient, TNetworkClient>
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
                        var response = (TResponse)Activator.CreateInstance(foundType);
                        response.Client = (TClient)this;
                        LoadResponse(response);
                        Socket.RegisterResponse(response);
                    }
                }
            }
        }

        protected virtual void Connected() { }
        private void Client_OnConnect(object sender, ClientEventArgs<TNetworkClient> e)
        {
            Connected();
            OnConnected.FireEvent(new BaseClientEventArgs<TClient, TNetworkClient>((TClient)this), this);
        }

        protected virtual void Disconnected(DisconnectReason reason) { }
        private void Client_OnDisconnect(object sender, DisconnectedEventArgs<TNetworkClient> e)
        {
            Disconnected(e.Reason);
            OnDisconnected.FireEvent(new BaseDisconnectedEventArgs<TClient, TNetworkClient>((TClient)this, e.Reason), this);
        }

        protected virtual void PacketWrite(IDataPacket packet) { }
        private void Client_OnPacketWrite(object sender, PacketEventArgs<TNetworkClient> e)
        { PacketWrite(e.Packet); }

        protected virtual void PacketSent(IDataPacket packet) { }
        private void Client_OnPacketSent(object sender, PacketEventArgs<TNetworkClient> e)
        { PacketSent(e.Packet); }

        protected virtual void PacketRead(IDataPacket packet) { }
        private void Client_OnPacketRead(object sender, PacketEventArgs<TNetworkClient> e)
        { PacketRead(e.Packet); }

        protected virtual void PacketReceive(IDataPacket packet) { }
        private void Client_OnPacketReceive(object sender, PacketEventArgs<TNetworkClient> e)
        { PacketReceive(e.Packet); }

        protected virtual void ErrorCaught(Exception ex) { }
        private void Socket_OnError(object sender, ClientErrorEventArgs<TNetworkClient> e)
        {
            ErrorCaught(e.Error);
            OnError.FireEvent(new BaseClientErrorEventArgs<TClient, TNetworkClient>((TClient)this, e.Error), this);
        }

        protected virtual void RequestWrite(IRequest<TNetworkClient> request) { }
        private void Socket_OnRequestWrite(object sender, RequestEventArgs<TNetworkClient> e)
        {
            if (e.Request is BaseRequest<TClient, TNetworkClient>)
                (e.Request as BaseRequest<TClient, TNetworkClient>).Client = (TClient)this;
            RequestWrite(e.Request);
        }

        protected virtual void ResponseExecute(ResponseCallEventArgs<TNetworkClient> e) { }
        private void Client_OnResponseExecute(object sender, ResponseCallEventArgs<TNetworkClient> e)
        {
            ResponseExecute(e);
        }

        public bool Equals(TClient other)
        {
            if (other == null)
                return false;
            return other.Socket.Equals(Socket);
        }
    }
}
