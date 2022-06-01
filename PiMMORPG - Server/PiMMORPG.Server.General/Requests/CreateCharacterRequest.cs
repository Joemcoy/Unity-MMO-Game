using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tFramework.Network.Interfaces;

namespace PiMMORPG.Server.General.Requests
{
    using Client;
    using Models;

    public class CreateCharacterRequest : PiBaseRequest
    {
        public override ushort ID => PacketID.CreateCharacter;

        public bool Result { get; set; }
        public Character[] Characters { get; set; }

        public override bool Write(IDataPacket packet)
        {
            packet.WriteBool(Result);

            if (Result)
            {
                packet.WriteInt(Characters.Length);
                foreach (var character in Characters)
                    character.WritePacket(packet);
            }
            return true;
        }
    }
}