using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tFramework.Network.Interfaces;

namespace PiMMORPG.Models
{
    public class AccessLevel : ModelBase
    {
        public string Name { get; set; }
        public string LevelColor { get; set; }
        public bool PanelAccess { get; set; }

        public override void ReadPacket(IDataPacket packet)
        {
            base.ReadPacket(packet);
            Name = packet.ReadString();
            LevelColor = packet.ReadString();
            PanelAccess = packet.ReadBool();
        }

        public override void WritePacket(IDataPacket packet)
        {
            base.WritePacket(packet);
            packet.WriteString(Name);
            packet.WriteString(LevelColor);
            packet.WriteBool(PanelAccess);
        }
    }
}