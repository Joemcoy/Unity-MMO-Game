using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PiMMORPG;
using PiMMORPG.Client;
using PiMMORPG.Models;
using tFramework.Network.Interfaces;

namespace Scripts.Network.Responses.GameClient
{
    using Local.Control;

    public class RemoveCharacterResponse : PiBaseResponse
    {
        public override ushort ID { get { return PacketID.RemoveCharacter; } }

        uint CharacterID;
        public override bool Read(IDataPacket Packet)
        {
            CharacterID = Packet.ReadUInt();

            return true;
        }

        public override void Execute()
        {
            WorldControl.RemovePlayer(Convert.ToInt32(CharacterID));
        }
    }
}