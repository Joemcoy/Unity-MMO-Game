using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Base.Data;
using Game.Data;
using Socket.Data;
using Socket.Data.Interfaces;
using Game.Data.Models;

namespace Game.Manager.Writers
{
    public class PutItemInWorldWriter : IPacketWriter
    {
        public uint ID { get { return PacketID.DataPutItemInWorld; } }
        public WorldItemModel Item { get; set; }

        public bool Write(IClientSocket Socket, SocketPacket Packet)
        {
            Packet.WriteInt32(Item.Group.ClaimLeader);
            Packet.WriteInt32(Item.ItemID);

            Item.Group.WritePacket(Packet);
            Item.Position.WritePacket(Packet);

            return true;
        }
    }
}
