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
    public class UpdateTimeWriter : IPacketWriter
    {
        public DateTime Time { get; set; }

        public uint ID { get { return PacketID.TimeUpdate; } }
        public bool Write(IClientSocket Client, SocketPacket Packet)
        {
            Packet.WriteDateTime(Time);
            return true;
        }
    }
}
