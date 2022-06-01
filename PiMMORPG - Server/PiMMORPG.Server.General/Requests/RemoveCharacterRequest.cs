using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tFramework.Network.Interfaces;

namespace PiMMORPG.Server.General.Requests
{
    using Client;
    using Models;

    public class RemoveCharacterRequest : PiBaseRequest
    {
        public override ushort ID => PacketID.RemoveCharacter;

        public uint CharacterId { get; set; }

        public override bool Write(IDataPacket packet)
        {
            packet.WriteUInt(CharacterId);
            return true;
        }
    }
}
