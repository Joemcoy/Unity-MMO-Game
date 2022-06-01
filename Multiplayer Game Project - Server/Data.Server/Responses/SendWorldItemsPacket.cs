using Base.Factories;
using Data.Client;
using Data.Server.Writers;
using Game.Controller;
using Game.Data;
using Game.Data.Models;
using Network.Data;
using Network.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Data.Server.Responses
{
    class SendWorldItemsPacket : DCResponse
    {
        public override uint ID { get { return PacketID.DataSendWorldItems; } }
        public override bool Read(ISocketPacket Packet)
        {
            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            ItemModel[] Items = ItemManager.GetAllItems();
            WorldItemModel[] WorldItems = WorldItemManager.GetAllItems();

            PutItemInWorldRequest Packet = new PutItemInWorldRequest();
            foreach(ItemModel Item in Items)
            {
                Packet.Item = Item;
                Client.Socket.Send(Packet);
            }
            
            /*Packet.Item = null;
            foreach(WorldItemModel Item in WorldItems)
            {
                Packet.WorldItem = Item;
                Client.Socket.Send(Packet);
            }*/
        }
    }
}
