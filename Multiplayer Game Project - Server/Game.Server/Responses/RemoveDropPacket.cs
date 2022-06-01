using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Game.Client;
using Game.Server.Manager;

using Network.Data.Interfaces;
using Base.Factories;
using Game.Data.Models;
using Game.Server.Writers;

namespace Game.Server.Responses
{
    public class RemoveDropPacket : GCResponse
    {
        public override uint ID { get { return PacketID.RemoveDrop; } }

        Guid Serial;
        public override bool Read(ISocketPacket Packet)
        {
            Serial = Packet.ReadGuid();
            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Manager = SingletonFactory.GetInstance<ItemCacheManager>();
            var Drop = WorldManager.RemoveDrop(Client.CurrentMap.ID, Serial);
            var Packet = new RemoveDropWriter();
            Packet.Serial = Serial;

            foreach (var Player in WorldManager.GetPlayersInMap(Client.CurrentCharacter.ID))
            {
                Player.Socket.Send(Packet);
            }

            var Item = Manager.GetItem(Drop.ItemID);
            var CItem = new CharacterItemModel();
            CItem.ItemID = Item.ID;
            CItem.OwnerID = Client.CurrentCharacter.ID;
            CItem.Serial = Serial;
            CItem.Amount = Drop.Amount;

            Manager.AddCharacterItem(CItem.OwnerID, CItem, true);
        }
    }
}
