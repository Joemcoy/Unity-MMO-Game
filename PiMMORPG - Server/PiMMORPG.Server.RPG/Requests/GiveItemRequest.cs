using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiMMORPG.Server.RPG.Requests
{
    using Models;
    using Client.RPG;
    using tFramework.Network.Interfaces;

    public class GiveItemRequest : PiRPGRequest
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
