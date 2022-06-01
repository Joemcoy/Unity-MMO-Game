using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Game.Data;
using Network.Data;
using Network.Data.Interfaces;
using Base.Factories;

namespace Game.Server.Writers
{
    public class KillPlayerWriter : IRequest
    {
        public uint ID { get { return PacketID.KillPlayer; } }
        public string Target { get; set; }
        public string Enemy { get; set; }
        public uint Level { get; set; }
        public uint Experience { get; set; }

        public bool Write(IClientSocket Client, ISocketPacket Packet)
        {
            Packet.WriteString(Target);
            Packet.WriteString(Enemy);
            Packet.WriteUInt(Level);
            Packet.WriteUInt(Experience);

            return true;
        }
    }
}
