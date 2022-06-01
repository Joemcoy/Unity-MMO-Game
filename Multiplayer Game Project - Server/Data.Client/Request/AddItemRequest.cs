using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Game.Data.Models;
using Network.Data.Interfaces;

namespace Data.Client.Requests
{
    public class AddItemRequest : IRequest
    {
        public uint ID { get { return PacketID.DataAddItem; } }
        public CharacterItemModel Item { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            Item.WritePacket(Packet);

            return true;
        }
    }
}
