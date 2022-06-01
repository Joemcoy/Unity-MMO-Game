using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Network.Data;
using Network.Data.Interfaces;
using Game.Data;
using Game.Data.Models;
using Game.Client;
using Data.Client;
using Game.Server.Manager;
using Base.Factories;
using Game.Server.Writers;

namespace Game.Server.DataResponses
{
    public class SendMapByIDPacket : DCResponse
    {
        MapModel Map;
        WorldItemModel[] Items;

        public override uint ID { get { return PacketID.DataSendMapByID; } }
        public override bool Read(ISocketPacket Packet)
        {
            if (Packet.ReadBool())
            {
                Map = new MapModel();
                Map.ReadPacket(Packet);

                Items = new WorldItemModel[0];
            }
            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            GameClient Client = this.Client.Dequeue<GameClient>();
            if (Map != null)
            {
                Client.CurrentMap = Map;
                LoggerFactory.GetLogger(this).LogInfo("Sending player to map {0}!", Map.Name);

                if (WorldManager.NotifyCharacter(Client, Client.CurrentCharacter))
                {
                    var Cache = SingletonFactory.GetInstance<ItemCacheManager>();

                    var Packet = new PlayerListWriter();
                    Packet.Map = Map;
                    Packet.CID = Client.CurrentCharacter.ID;
                    Packet.Items = Cache.GetWorldItems(Map.ID);
                    Client.Socket.Send(Packet);
                }

                var DropPacket = new DropItemWriter();
                var Manager = SingletonFactory.GetInstance<ItemCacheManager>();

                /*foreach(var Drop in WorldManager.GetDrops(Map.ID))
                {
                    DropPacket.InventoryID = (uint)Manager.GetItem(Drop.ItemID).UniqueID;
                    DropPacket.Position = Drop.Position;
                    DropPacket.Serial = Drop.Serial;
                    DropPacket.Amount = Drop.Amount;

                    Client.Socket.Send(DropPacket);
                }*/
            }
            else
                LoggerFactory.GetLogger(this).LogWarning("Map not found!");
        }
    }
}
