using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Base.Data;
using Game.Data;
using Network.Data;
using Network.Data.Interfaces;
using Game.Data.Models;

namespace Game.Server.Writers
{
    public class PutItemInWorldWriter : IRequest
    {
        public uint ID { get { return PacketID.PutItemInWorld; } }
        public WorldItemModel Item { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            Item.WritePacket(Packet);
            return true;
        }
    }
}
