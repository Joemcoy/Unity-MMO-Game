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
    public class AttackPlayerWriter : IRequest
    {
        public uint ID { get { return PacketID.AttackPlayer; } }
        public string NameA { get; set; }
        public string NameB { get; set; }
        public uint Damage { get; set; }
        public bool Front { get; set; }

        public bool Write(IClientSocket Client, ISocketPacket Packet)
        {
            Packet.WriteString(NameA);
            Packet.WriteString(NameB);
            Packet.WriteUInt(Damage);
            Packet.WriteBool(Front);

            return true;
        }
    }
}
