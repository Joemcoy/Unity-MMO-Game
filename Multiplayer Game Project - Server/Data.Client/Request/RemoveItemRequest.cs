using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Game.Data.Models;
using Network.Data.Interfaces;

namespace Data.Client.Requests
{
    public class RemoveItemRequest : IRequest
    {
        public uint ID { get { return PacketID.DataRemoveItem; } }
        public Guid Serial { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            Packet.WriteGuid(Serial);

            return true;
        }
    }
}
