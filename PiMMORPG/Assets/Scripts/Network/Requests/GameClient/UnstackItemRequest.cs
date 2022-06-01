using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PiMMORPG;
using PiMMORPG.Client;
using tFramework.Network.Interfaces;

namespace Scripts.Network.Requests.GameClient
{
    public class UnstackItemRequest : PiBaseRequest
    {
        public override ushort ID { get { return PacketID.UnstackItems; } }
        public Guid From { get; set; }
        public uint FromSlot { get; set; }
        public Guid To { get; set; }
        public uint ToSlot { get; set; }
        public uint Quantity { get; set; }

        public override bool Write(IDataPacket packet)
        {
            packet.WriteGuid(From);
            packet.WriteUInt(FromSlot);
            packet.WriteGuid(To);
            packet.WriteUInt(ToSlot);
            packet.WriteUInt(Quantity);
            return From != Guid.Empty && To != Guid.Empty;
        }
    }
}
