using System;
using System.Net;
using System.Net.Sockets;
using System.IO;

using Network.Data.EventArgs;
using System.Reflection;

namespace Network.Data.Interfaces
{
    public interface IClientSocket : IEquatable<IClientSocket>
    {
        event EventHandler<ClientSocketEventArgs> Connected, ResponsesLoaded, PingReceived;
        event EventHandler<ClientDisconnectedEventArgs> Disconnected;
        event EventHandler<PacketEventArgs> PacketReading, PacketReceived, PacketSending, PacketSent;
        event EventHandler<RequestEventArgs> RequestSending, RequestSent;
        event EventHandler<ResponseEventArgs> ResponseLoaded, ResponseReading, ResponseRead;
        event EventHandler<ClientExceptionEventArgs> ErrorThrowed;

        NetworkStream Stream { get; }
        IResponse[] Responses { get; }
        IPEndPoint EndPoint { get; set; }
        IServerSocket Server { get; }
        bool IOEnabled { get; set; }
        Exception LastError { get; }

        bool IsConnected { get; }
        int Ping { get; }

        bool Connect();
        bool Disconnect();

        bool Send(IRequest Request);
        bool Send(ISocketPacket Packet);

        bool RegisterResponse<TResponse>() where TResponse : IResponse;
        bool RegisterResponse<TResponse>(params Assembly[] Assemblies) where TResponse : IResponse;
        void ParseDNS(string Address);
        void ParseDNS(string Address, int Port);
    }
}

