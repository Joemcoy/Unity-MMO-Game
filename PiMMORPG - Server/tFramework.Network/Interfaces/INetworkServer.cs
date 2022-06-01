using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;


namespace tFramework.Network.Interfaces
{
    using EventArgs;
    using Enums;

    public interface INetworkServer<TNetworkClient>
        where TNetworkClient : INetworkClient<TNetworkClient>, new()
    {
        IPEndPoint EndPoint { get; set; }
        TNetworkClient[] Clients { get; }
        bool Opened { get; }
        uint ClientCount { get; }
        Type PacketType { get; set; }

        bool Open();
        bool Close();
        void DisconnectAll();
        void SendToAll(IDataPacket packet, Predicate<TNetworkClient> condition = null);
        void SendToAll(IRequest<TNetworkClient> request, Predicate<TNetworkClient> condition = null);
        void FireDisconnected(TNetworkClient client);
    }

    public interface INetworkServer<TNetworkServer, TNetworkClient> : INetworkServer<TNetworkClient>
        where TNetworkServer : INetworkServer<TNetworkServer, TNetworkClient>, new()
        where TNetworkClient : INetworkClient<TNetworkClient>, new()
    {
        event EventHandler<ServerEventArgs<TNetworkServer, TNetworkClient>> OnOpen, OnClose;
        event EventHandler<ClientEventArgs<TNetworkClient>> OnConnected;
    }
}