using System;

namespace Network.Data.Interfaces
{
    public interface IResponse : IPacketHandler
    {
        bool Read(ISocketPacket Packet);
        void Execute(IClientSocket Client);
    }
}

