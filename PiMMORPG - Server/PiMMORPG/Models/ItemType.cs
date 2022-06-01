using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tFramework.Network.Interfaces;

namespace PiMMORPG.Models
{
    public class ItemType : ModelBase
    {
        public string Name { get; set; }
        public int InventoryID { get; set; }

        public override void ReadPacket(IDataPacket packet)
        {
            base.ReadPacket(packet);
            Name = packet.ReadString();
            InventoryID = packet.ReadInt();
        }

        public override void WritePacket(IDataPacket packet)
        {
            base.WritePacket(packet);
            packet.WriteString(Name);
            packet.WriteInt(InventoryID);
        }
    }
}