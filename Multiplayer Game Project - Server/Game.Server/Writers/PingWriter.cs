using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Network.Data;
using Network.Data.Interfaces;
using Game.Data;

namespace Game.Server.Writers
{
    public class PingWriter : IRequest
    {
        public uint ID { get { return PacketID.Ping; } }

        public bool Write(IClientSocket Client, ISocketPacket Packet)
        {
            return true;
        }
    }
}
