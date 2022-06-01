using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Game.Client;

using Network.Data.Interfaces;
using Game.Server.Writers;
using Game.Server.Manager;
using Base.Factories;

namespace Game.Server.Responses
{
    public class SendDropsPacket : GCResponse
    {
        public override uint ID { get { return PacketID.SendDrops; } }

        public override bool Read(ISocketPacket Packet)
        {
            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Drops = WorldManager.GetDrops(Client.CurrentMap.ID);
            var Manager = SingletonFactory.GetInstance<ItemCacheManager>();

            var Packet = new DropItemWriter();
            foreach (var Drop in Drops)
            {
                Packet.Serial = Drop.Serial;
                Packet.Position = Drop.Position;
                Packet.InventoryID = (uint)Manager.GetItem(Drop.ItemID).UniqueID;
                Packet.Amount = Drop.Amount;

                Socket.Send(Packet);
            }
        }
    }
}
