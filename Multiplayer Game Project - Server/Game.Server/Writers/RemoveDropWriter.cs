using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Network.Data.Interfaces;

namespace Game.Server.Writers
{
    public class RemoveDropWriter : IRequest
    {
        public uint ID { get { return PacketID.RemoveDrop; } }
        public Guid Serial { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            Packet.WriteGuid(Serial);

            return true;
        }
    }
}
