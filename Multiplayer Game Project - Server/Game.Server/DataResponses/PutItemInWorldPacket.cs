using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Data.Client;
using Base.Data;
using Game.Data;
using Network.Data;
using Network.Data.Interfaces;
using Game.Data.Models;
using Game.Client;
using Base.Factories;
using Game.Server.Writers;
using Game.Server.Manager;

namespace Game.Server.DataResponses
{
    public class PutItemInWorldPacket : DCResponse
    {
        ItemModel Item;
        WorldItemModel WorldItem;

        public override uint ID { get { return PacketID.DataPutItemInWorld; } }
        public override bool Read(ISocketPacket Packet)
        {
            if (Packet.ReadBool())
            {
                Item = null;
                WorldItem = new WorldItemModel();
                WorldItem.ReadPacket(Packet);
            }
            else
            {
                WorldItem = null;
                Item = new ItemModel();
                Item.ReadPacket(Packet);
            }

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            ItemCacheManager Cache = SingletonFactory.GetInstance<ItemCacheManager>();

            if (Item == null)
                Cache.PutWorldItem(WorldItem);
            else
                Cache.PutItem(Item);
        }
    }
}