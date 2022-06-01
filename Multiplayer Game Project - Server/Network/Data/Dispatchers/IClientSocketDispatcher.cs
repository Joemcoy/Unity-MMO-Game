using System;
using Base.Data.Interfaces;

using Network.Data.Enums;
using Network.Data.Interfaces;

namespace Network.Data.Dispatchers
{
    public interface IClientSocketDispatcher : IDispatcherBase
    {
        void OnConnect(IClientSocket Client);
        void OnDisconnect(IClientSocket Client, DisconnectReason Reason);

        void OnPacketReceived(IClientSocket Client, ISocketPacket Packet);
        void OnPacketRead(IClientSocket Client, ISocketPacket Packet);

        void OnPacketSending(IClientSocket Client, ISocketPacket Packet);
        void OnPacketSent(IClientSocket Client, ISocketPacket Packet);

        void OnRequestSending(IClientSocket Client, IRequest Request);
        void OnResponseReceived(IClientSocket Client, IResponse Response);

        void OnError(IClientSocket Client, Exception Error);
        void OnResponseLoad(IClientSocket Client, IResponse Response);
    }
}

