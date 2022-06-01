using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tFramework.Network.Interfaces;

namespace PiMMORPG.Server.General.Requests
{
    using Client;
    using Models;

    public class SpawnCharacterRequest : PiBaseRequest
    {
        public override ushort ID => PacketID.SpawnCharacter;

        public Character Character { get; set; }

        public override bool Write(IDataPacket packet)
        {
            packet.WriteWrapper(Character);
            packet.WriteWrappers(Character.Items.Where(i => i.Equipped).ToArray());
            return true;
        }
    }
}
