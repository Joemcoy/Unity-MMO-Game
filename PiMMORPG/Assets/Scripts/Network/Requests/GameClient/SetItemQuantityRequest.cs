using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PiMMORPG;
using PiMMORPG.Models;
using PiMMORPG.Client;
using tFramework.Network.Interfaces;

namespace Scripts.Network.Requests.GameClient
{
    public class SetItemQuantityRequest : PiBaseRequest
    {
        public override ushort ID { get { return PacketID.SetItemQuantity; } }

        public Guid Serial { get; set; }
        public uint Quantity { get; set; }
        public override bool Write(IDataPacket packet)
        {
            packet.WriteGuid(Serial);
            packet.WriteUInt(Quantity);
            return Serial != Guid.Empty;
        }
    }
}