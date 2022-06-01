using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PiMMORPG;
using PiMMORPG.Client;

using tFramework.Factories;
using tFramework.Network.Interfaces;

namespace Scripts.Network.Requests.GameClient
{
    public class SetHotbarSlotRequest : PiBaseRequest
    {
        public override ushort ID { get { return PacketID.SetHotbarSlot; } }
        public Guid Serial { get; set; }
        public int Slot { get; set; }

        public override bool Write(IDataPacket packet)
        {
            packet.WriteGuid(Serial);
            packet.WriteInt(Slot);
            return Serial != Guid.Empty && PiBaseClient.IsLoaded;
        }
    }
}