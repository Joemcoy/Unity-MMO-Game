using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PiMMORPG;
using PiMMORPG.Client;
using tFramework.Network.Interfaces;

namespace Scripts.Network.Requests.GameClient
{
    public class RemoveItemRequest : PiBaseRequest
    {
        public override ushort ID { get { return PacketID.RemoveItem; } }

        public Guid Serial { get; set; }
        public override bool Write(IDataPacket packet)
        {
            packet.WriteGuid(Serial);
            return Serial != Guid.Empty;
        }
    }
}
