using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PiMMORPG;
using PiMMORPG.Client;
using tFramework.Network.Interfaces;

namespace Scripts.Network.Requests.GameClient
{
    public class MergeItemRequest : PiBaseRequest
    {
        public override ushort ID { get { return PacketID.MergeItems; } }

        public Guid From { get; set; }
        public Guid To { get; set; }
        public uint Quantity { get; set; }

        public override bool Write(IDataPacket packet)
        {
            packet.WriteGuid(From);
            packet.WriteGuid(To);
            packet.WriteUInt(Quantity);
            return From != Guid.Empty && To != Guid.Empty;
        }
    }
}
