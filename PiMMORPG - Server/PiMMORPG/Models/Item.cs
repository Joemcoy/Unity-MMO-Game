using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tFramework.Network.Interfaces;

namespace PiMMORPG.Models
{
    public class Item : ModelBase
    {
        public uint InventoryID { get; set; }
        public ItemType Type { get; set; }
        public bool IsEquip { get; set; }

        public override void ReadPacket(IDataPacket packet)
        {
            base.ReadPacket(packet);
            InventoryID = packet.ReadUInt();
            Type = packet.ReadWrapper<ItemType>();
            IsEquip = packet.ReadBool();
        }

        public override void WritePacket(IDataPacket packet)
        {
            base.WritePacket(packet);
            packet.WriteUInt(InventoryID);
            packet.WriteWrapper(Type);
            packet.WriteBool(IsEquip);
        }
    }
}