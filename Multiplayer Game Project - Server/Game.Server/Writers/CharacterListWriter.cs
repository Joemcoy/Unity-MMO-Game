using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Game.Data;
using Game.Data.Models;
using Server.Configuration;

using Network.Data;
using Network.Data.Interfaces;
using Base.Factories;
using Game.Server.Manager;

namespace Game.Server.Writers
{
    public class CharacterListWriter : IRequest
    {
        public CharacterModel[] Characters { get; set; }
        public CharacterItemModel[] Items { get; set; }

        public uint ID { get { return PacketID.CharacterList; } }
        public bool Write(IClientSocket Client, ISocketPacket Packet)
        {
            Packet.WriteInt(GameConfiguration.MaximumCharacters);
            Packet.WriteUInt(GameConfiguration.BaseExperience);

            Packet.WriteBool(Characters.Length > 0);
            if (Characters.Length > 0)
            {
                var Manager = SingletonFactory.GetInstance<ItemCacheManager>();
                var RealItems = Manager.GetItems().Where(I => Items.Any(C => C.ItemID == I.ID));

                Packet.WriteInt(RealItems.Count());
                foreach (var Item in RealItems)
                    Item.WritePacket(Packet);

                Packet.WriteInt(Characters.Length);
                foreach (CharacterModel Character in Characters)
                {
                    Character.WritePacket(Packet);

                    var Items = this.Items.Where(I => I.OwnerID == Character.ID);
                    Packet.WriteInt(Items.Count());

                    foreach (var Item in Items)
                        Item.WritePacket(Packet);
                }
            }
            return true;
        }
    }
}