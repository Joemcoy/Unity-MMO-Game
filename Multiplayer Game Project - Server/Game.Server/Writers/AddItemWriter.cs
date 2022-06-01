using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Game.Data.Models;
using Network.Data.Interfaces;

namespace Game.Server.Writers
{
    public class AddItemWriter : IRequest
    {
        public uint ID { get { return PacketID.AddItem; } }
        public CharacterItemModel Item { get; set; }
        public ItemModel RealItem { get; set; }

        public bool Write(IClientSocket Client, ISocketPacket Packet)
        {
            Item.WritePacket(Packet);
            RealItem.WritePacket(Packet);
            return true;
        }
    }
}
