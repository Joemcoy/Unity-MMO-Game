using System;
using System.Net;

using Network.Data.EventArgs;

namespace Network.Data.Interfaces
{
    public interface IServerSocket
    {
        event EventHandler<ServerSocketEventArgs> Opened, Closed;
        event EventHandler<ClientConnectedEventArgs> ClientConnected;
        event EventHandler<ServerExceptionEventArgs> ErrorThrowed;

        IPEndPoint EndPoint { get; set; }
        IClientSocket[] Clients { get; }

        bool Open();
        bool Close();

        bool SendToAll(ISocketPacket Packet);
        bool SendToAll(IRequest Request);
        bool DisconnectAll();

        void FireClientDisconnected(IClientSocket Client);
    }
}

