using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tFramework.Network.Interfaces;

namespace PiMMORPG.Server.General.Requests
{
    using Client;
    using Drivers;
    using Models;

    public class SendCharacterRequest : PiBaseRequest
    {
        public override ushort ID => PacketID.SendCharacter;

        public Character Character { get; set; }
        public CharacterItem[] Items { get; set; }

        public bool Result { get; set; }
        public override bool Write(IDataPacket packet)
        {
            packet.WriteBool(Result);

            if (Result)
            {
                packet.WriteWrapper(Character);
                packet.WriteWrappers(Items);
            }
            return true;
        }
    }
}
