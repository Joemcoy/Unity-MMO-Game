using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiMMORPG.Server.General.Requests
{
    using Client;
    using Models;
    using tFramework.Network.Interfaces;

    public class SendDropRequest : PiBaseRequest
    {
        public override ushort ID => PacketID.DropItem;
        public Drop Drop { get; set; }

        public override bool Write(IDataPacket packet)
        {
            packet.WriteWrapper(Drop);
            return true;
        }
    }
}