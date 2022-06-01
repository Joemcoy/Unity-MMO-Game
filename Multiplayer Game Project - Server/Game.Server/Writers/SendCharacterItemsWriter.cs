using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Network.Data.Interfaces;

using Game.Data;
using Game.Data.Models;
using Base.Factories;
using Game.Server.Manager;

namespace Game.Server.Writers
{
    public class SendCharacterItemsWriter : IRequest
    {
        public uint ID { get { return PacketID.SendCharacterItems; } }

        public int Owner { get; set; }
        public CharacterItemModel[] Items { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            var Server = SingletonFactory.GetInstance<GameServer>();
            var C1 = Server.Clients.First(C => C.Socket == Socket).CurrentCharacter;

            LoggerFactory.GetLogger(this).LogWarning("Sending items of character {0} to character {1}!", Owner, C1.ID);

            Packet.WriteInt(Owner);
            Packet.WriteInt(this.Items.Length);
            foreach (var item in this.Items)
            {
                if(C1.ID == Owner || item.Equiped)
                    item.WritePacket(Packet);
            }

            var Cache = SingletonFactory.GetInstance<ItemCacheManager>();
            var Items = Cache.GetItems();

            Packet.WriteInt(Items.Length);
            foreach (var Item in Items)
                Item.WritePacket(Packet);

            return true;
        }
    }
}
