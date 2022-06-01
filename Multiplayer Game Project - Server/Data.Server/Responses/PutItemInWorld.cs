using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Base.Factories;
using Data.Client;
using Network.Data;
using Network.Data.Interfaces;
using Game.Data;
using Game.Data.Models;
using Game.Controller;

namespace Data.Server.Responses
{
    public class PutItemInWorld : DCResponse
    {
        int ClaimLeader;
        int ItemID;

        WorldItemGroupModel Group;
        PositionModel Position;

        public override uint ID { get { return PacketID.DataPutItemInWorld; } }
        public override bool Read(ISocketPacket Packet)
        {
            try
            {
                ClaimLeader = Packet.ReadInt();
                ItemID = Packet.ReadInt();

                Group = new WorldItemGroupModel();
                Group.ReadPacket(Packet);

                Position = new PositionModel();
                Position.ReadPacket(Packet);

                return true;
            }
            catch(Exception ex)
            {
                LoggerFactory.GetLogger(this).LogFatal(ex);
                return false;
            }
        }

        public override void Execute(IClientSocket Socket)
        {
            WorldItemModel Item = new WorldItemModel();
            
            Item.ItemID = ItemID;
            Item.GroupID = Group.ID;
            if (Item.GroupID == 0)
                Item.GroupID = 1;

            Item.Group = Group;
            Item.Group.ClaimLeader = ClaimLeader;

            Item.Item = ItemManager.GetItemByID(ItemID);
            Item.Position = Position;

            WorldItemManager.AddItem(Item);

            /*SocketPacket Packet = StreamPacket(PacketID.DataPutItemInWorld);
            Item.WritePacket(Packet);
            Client.Socket.Send(Packet);*/
        }
    }
}
