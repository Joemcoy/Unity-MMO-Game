using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;

namespace tFramework.Network.Interfaces
{
    using EventArgs;
    public interface INetworkClient<TNetworkClient> : IEquatable<TNetworkClient>
        where TNetworkClient : INetworkClient<TNetworkClient>, new()
    {
        event EventHandler<ClientEventArgs<TNetworkClient>> OnConnect;
        event EventHandler<DisconnectedEventArgs<TNetworkClient>> OnDisconnect;
        event EventHandler<PacketEventArgs<TNetworkClient>> OnPacketRead, OnPacketReceive, OnPacketWrite, OnPacketSent;
        event EventHandler<RequestEventArgs<TNetworkClient>> OnRequestWrite;
        event EventHandler<ResponseCallEventArgs<TNetworkClient>> OnResponseExecute;
        event EventHandler<ClientErrorEventArgs<TNetworkClient>> OnError;

        IResponse<TNetworkClient>[] Responses { get; }
        bool IOEnabled { get; set; }
        bool Connected { get; }
        Type PacketType { get; set; }
        INetworkServer<TNetworkClient> Server { get; }
        IPEndPoint EndPoint { get; set; }
        SocketType SocketType { get; set; }
        ProtocolType Protocol { get; set; }

        bool Connect();
        bool Disconnect();
        bool Resolve(string Address);
        void RegisterResponse(IResponse<TNetworkClient> response);
        void Send(IDataPacket packet);
        void Send(IRequest<TNetworkClient> request);
        IDataPacket CreatePacket(ushort ID);
    }
}