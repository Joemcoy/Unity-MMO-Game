using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PiMMORPG;
using PiMMORPG.Models;
using PiMMORPG.Client;

using tFramework.Factories;
using tFramework.Network.Interfaces;

namespace Scripts.Network.Requests.GameClient
{
    public class SetEquipStateRequest : PiBaseRequest
    {
        public override ushort ID { get { return PacketID.SetEquipState; } }

        public Guid Serial { get; set; }
        public bool Equipped { get; set; }
        public override bool Write(IDataPacket packet)
        {
            packet.WriteGuid(Serial);
            packet.WriteBool(Equipped);
            return Serial != Guid.Empty && PiBaseClient.IsLoaded;
        }
    }
}