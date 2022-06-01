using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Game.Data.Models;
using Network.Data.Interfaces;

namespace Data.Server.Writers
{
    public class CharacterItemsRequest : IRequest
    {
        public uint ID { get { return PacketID.DataSendCharacterItems; } }
        public int CharacterID { get; set; }
        public CharacterItemModel[] Items { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            Packet.WriteInt(CharacterID);
            Packet.WriteInt(Items.Length);

            foreach (var Item in Items)
                Item.WritePacket(Packet);

            return true;
        }
    }
}
