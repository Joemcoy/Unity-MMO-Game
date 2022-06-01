using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Game.Data;
using Socket.Data;
using Socket.Data.Interfaces;

namespace Game.Manager.Writers
{
    public class AlreadyOnlineWriter : IPacketWriter
    {
        public uint ID { get { return PacketID.AlreadyOnline; } }

        public bool Write(IClientSocket Client, SocketPacket Packet)
        {
            return true;
        }
    }
}
