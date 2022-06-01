using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tFramework.Network.Interfaces;

namespace PiMMORPG.Models
{
    public class Tree : SerialModelBase
    {
        public string BundleName { get; set; }
        public Map Map { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }

        public override void ReadPacket(IDataPacket packet)
        {
            base.ReadPacket(packet);
            BundleName = packet.ReadString();
            Map = packet.ReadWrapper<Map>();
            PositionX = packet.ReadFloat();
            PositionY = packet.ReadFloat();
            PositionZ = packet.ReadFloat();
        }

        public override void WritePacket(IDataPacket packet)
        {
            base.WritePacket(packet);
            packet.WriteString(BundleName);
            packet.WriteWrapper(Map);
            packet.WriteFloat(PositionX);
            packet.WriteFloat(PositionY);
            packet.WriteFloat(PositionZ);
        }
    }
}