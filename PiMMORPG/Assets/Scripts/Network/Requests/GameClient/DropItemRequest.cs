using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PiMMORPG;
using PiMMORPG.Client;
using PiMMORPG.Models;
using tFramework.Network.Interfaces;

namespace Scripts.Network.Requests.GameClient
{
    public class DropItemRequest : PiBaseRequest
    {
        public override ushort ID { get { return PacketID.DropItem; } }
        public Drop Drop { get; set; }

        public override bool Write(IDataPacket packet)
        {
            if (Drop != null && Drop.Serial != Guid.Empty)
            {
                packet.WriteWrapper(Drop);
                return true;
            }
            return false;
        }
    }
}