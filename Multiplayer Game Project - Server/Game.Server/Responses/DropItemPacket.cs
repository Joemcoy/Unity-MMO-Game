using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Base.Factories;
using Game.Data;
using Game.Data.Models;

using Game.Client;
using Game.Server.Manager;

using Network.Data.Interfaces;
using Game.Server.Writers;

namespace Game.Server.Responses
{
    public class DropItemPacket : GCResponse
    {
        public override uint ID { get { return PacketID.DropItem; } }

        uint ItemID, Amount;
        PositionModel Position;
        Guid Serial;

        public override bool Read(ISocketPacket Packet)
        {
            ItemID = Packet.ReadUInt();
            Amount = Packet.ReadUInt();
            Position = new PositionModel();
            Position.ReadPacket(Packet);
            Position.MapID = Client.CurrentMap.ID;
            Serial = Packet.ReadGuid();

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            LoggerFactory.GetLogger(this).LogWarning("Dropping item {0} on map {1}!", ItemID, Position.MapID);

            var Manager = SingletonFactory.GetInstance<ItemCacheManager>();

            Manager.RemoveItem(Client.CurrentCharacter.ID, Serial);

            var Drop = new DropModel();
            Drop.ItemID = Manager.GetItems().First(I => (uint)I.UniqueID == ItemID).ID;
            Drop.Position = Position;
            Drop.Serial = Serial;
            Drop.Amount = Amount;

            WorldManager.AddDrop(Drop);

            var Packet = new DropItemWriter();
            Packet.InventoryID = ItemID;
            Packet.Position = Position;
            Packet.Serial = Serial;
            Packet.Amount = Amount;

            foreach (var Client in WorldManager.GetPlayersInMap(Position.MapID))
            {
                Client.Socket.Send(Packet);
            }
        }
    }
}