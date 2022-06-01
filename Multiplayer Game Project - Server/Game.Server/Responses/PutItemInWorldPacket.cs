using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Base.Data;
using Base.Factories;

using Network.Data.Interfaces;

using Game.Client;

using Game.Data;
using Game.Data.Models;
using Game.Server.Manager;
using Game.Server.Writers;

namespace Game.Server.Responses
{
    public class PutItemInWorldPacket : GCResponse
    {
        int ItemID;

        WorldItemGroupModel Group;
        PositionModel Position;

        public override uint ID { get { return PacketID.PutItemInWorld; } }
        public override bool Read(ISocketPacket Packet)
        {
            ItemID = Packet.ReadInt();
            Group = new WorldItemGroupModel();
            Group.ReadPacket(Packet);

            Position = new PositionModel();
            Position.ReadPacket(Packet);
            
            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            ItemCacheManager Cache = SingletonFactory.GetInstance<ItemCacheManager>();

            WorldItemModel Item = new WorldItemModel();
            Item.Item = Cache.GetItem(ItemID);
            Item.ItemID = ItemID;
            Item.Group = Group;
            Item.Position = Position;
            Item.Group.ClaimLeader = Client.CurrentCharacter.ID;
            Cache.AddWorldItem(Item);

            var Packet = new PutItemInWorldWriter();
            Packet.Item = Item;

            GameServer Server = SingletonFactory.GetInstance<GameServer>();
            foreach (GameClient RClient in Server.Clients)
            {
                LoggerFactory.GetLogger(this).LogInfo("Sending item to {0}", Client.Socket.EndPoint);
                RClient.Socket.Send(Packet);
            }
        }
    }
}