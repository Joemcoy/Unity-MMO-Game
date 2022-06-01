using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Data.Interfaces;
using tFramework.Network.Interfaces;

namespace PiMMORPG.Models
{
    public class Drop : Position, ISerialModel
    {
        public Guid Serial { get; set; }
        public Map Map { get; set; }
        public uint InventoryID { get; set; }
        public uint Quantity { get; set; }

        public override void ReadPacket(IDataPacket packet)
        {
            base.ReadPacket(packet);
            Serial = packet.ReadGuid();
            //Map = packet.ReadWrapper<Map>();
            InventoryID = packet.ReadUInt();
            Quantity = packet.ReadUInt();
        }

        public override void WritePacket(IDataPacket packet)
        {
            base.WritePacket(packet);
            packet.WriteGuid(Serial);
            //packet.WriteWrapper(Map);
            packet.WriteUInt(InventoryID);
            packet.WriteUInt(Quantity);
        }
    }
}