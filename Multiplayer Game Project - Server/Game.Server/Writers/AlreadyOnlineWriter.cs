using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Game.Data;
using Network.Data;
using Network.Data.Interfaces;

namespace Game.Server.Writers
{
    public class AlreadyOnlineWriter : IRequest
    {
        public uint ID { get { return PacketID.AlreadyOnline; } }

        public bool Write(IClientSocket Client, ISocketPacket Packet)
        {
            return true;
        }
    }
}
