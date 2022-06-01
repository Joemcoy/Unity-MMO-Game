using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Game.Data.Models;
using Network.Data.Interfaces;

namespace Game.Server.Writers
{
    public class DropItemWriter : IRequest
    {
        public uint ID { get { return PacketID.DropItem; } }
        public uint InventoryID { get; set; }
        public uint Amount { get; set; }
        public PositionModel Position { get; set; }
        public Guid Serial { get; set; }

        public DropModel Drop
        {
            set
            {
                InventoryID = (uint)value.ItemID;
                Amount = value.Amount;
                Position = value.Position;
                Serial = value.Serial;
            }
        }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            Base.Factories.LoggerFactory.GetLogger(this).LogWarning("Sending drop of item {0}!", InventoryID);
            Packet.WriteUInt(InventoryID);
            Packet.WriteUInt(Amount);
            Position.WritePacket(Packet);
            Packet.WriteGuid(Serial);

            return true;
        }
    }
}
