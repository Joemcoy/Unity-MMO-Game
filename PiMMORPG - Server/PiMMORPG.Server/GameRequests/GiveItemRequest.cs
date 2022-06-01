using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiMMORPG.Server.General.Requests
{
    using Models;
    using Client;
    using tFramework.Network.Interfaces;

    public class GiveItemRequest : PiBaseRequest
    {
        public override ushort ID => PacketID.GiveItem;

        public CharacterItem Item { get; set; }
        public override bool Write(IDataPacket packet)
        {
            packet.WriteWrapper(Item);
            return true;
        }
    }
}
