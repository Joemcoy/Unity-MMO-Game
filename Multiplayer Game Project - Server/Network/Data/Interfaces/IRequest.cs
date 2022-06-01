using System;
namespace Network.Data.Interfaces
{
    public interface IRequest : IPacketHandler
    {
        bool Write(IClientSocket Client, ISocketPacket Packet);
    }
}

