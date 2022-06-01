using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tFramework.Network.Interfaces;

namespace PiMMORPG.Models
{
    public class CharacterItem : SerialModelBase
    {
        public uint Character { get; set; }
        public Item Info { get; set; }
        public uint Slot { get; set; }
        public uint Quantity { get; set; }
        public int HotbarSlot { get; set; }
        public bool Equipped { get; set; }

        public CharacterItem()
        {
            Equipped = false;
            HotbarSlot = -1;
        }

        public override void ReadPacket(IDataPacket packet)
        {
            base.ReadPacket(packet);
            Character = packet.ReadUInt();
            Info = packet.ReadWrapper<Item>();
            Slot = packet.ReadUInt();
            Quantity = packet.ReadUInt();
            HotbarSlot = packet.ReadInt();
            Equipped = packet.ReadBool();
        }

        public override void WritePacket(IDataPacket packet)
        {
            base.WritePacket(packet);
            packet.WriteUInt(Character);
            packet.WriteWrapper(Info);
            packet.WriteUInt(Slot);
            packet.WriteUInt(Quantity);
            packet.WriteInt(HotbarSlot);
            packet.WriteBool(Equipped);
        }
    }
}