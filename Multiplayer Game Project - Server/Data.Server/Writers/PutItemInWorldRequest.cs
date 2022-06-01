using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Network.Data;
using Network.Data.Interfaces;
using Game.Data;
using Game.Data.Models;

namespace Data.Server.Writers
{
    public class PutItemInWorldRequest : IRequest
    {
        public uint ID { get { return PacketID.DataPutItemInWorld; } }

        public ItemModel Item { get; set; }
        public WorldItemModel WorldItem { get; set; }
        public bool End { get; set; }

        public bool Write(IClientSocket Client, ISocketPacket Packet)
        {
            if (Item == null)
            {
                Packet.WriteBool(true);
                WorldItem.WritePacket(Packet);
            }
            else
            {
                Packet.WriteBool(false);
                Item.WritePacket(Packet);
            }
            return true;
        }
    }
}
