using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Game.Data.Models;
using Network.Data.Interfaces;

namespace Data.Server.Writers
{
    public class SendEquipsRequest : IRequest
    {
        public uint ID { get { return PacketID.DataSendEquips; } }
        public CharacterItemModel[] Items { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            Packet.WriteInt(Items.Length);

            foreach (var Item in Items)
                Item.WritePacket(Packet);

            return true;
        }
    }
}
