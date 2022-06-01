using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tFramework.Network.Interfaces;

namespace PiMMORPG.Models
{
    public class Spawn : Position
    {
        public int MapID { get; set; }

        public override void ReadPacket(IDataPacket packet)
        {
            base.ReadPacket(packet);
            MapID = packet.ReadInt();
        }

        public override void WritePacket(IDataPacket packet)
        {
            base.WritePacket(packet);
            packet.WriteInt(MapID);
        }
    }
}