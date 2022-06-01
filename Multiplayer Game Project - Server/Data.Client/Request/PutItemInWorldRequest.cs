using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Game.Data.Models;
using Network.Data.Interfaces;

namespace Data.Client.Requests
{
    public class PutItemInWorldRequest : IRequest
    {
        public uint ID { get { return PacketID.DataPutItemInWorld; } }
        public int ClaimLeader { get; set; }
        public int ItemID { get; set; }
        public WorldItemGroupModel Group { get; set; }
        public PositionModel Position { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            Packet.WriteInt(ClaimLeader);
            Packet.WriteInt(ItemID);
            Group.WritePacket(Packet);
            Position.WritePacket(Packet);

            return true;
        }
    }
}
