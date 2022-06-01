using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiMMORPG.Server.General.Requests
{
    using Client;
    using tFramework.Network.Interfaces;

    public class RemoveDropRequest : PiBaseRequest
    {
        public override ushort ID => PacketID.RemoveDrop;

        public Guid Serial { get; set; }
        public override bool Write(IDataPacket packet)
        {
            if(Serial != Guid.Empty)
            {
                packet.WriteGuid(Serial);
                return true;
            }
            return false;
        }
    }
}