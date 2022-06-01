using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PiMMORPG;
using PiMMORPG.Models;
using PiMMORPG.Client;
using tFramework.Network.Interfaces;

namespace Scripts.Network.Requests.GameClient
{
    public class SelectCharacterRequest : PiBaseRequest
    {
        public override ushort ID { get { return PacketID.SelectCharacter; } }

        public uint CharacterID { get; set; }
        public override bool Write(IDataPacket Packet)
        {
            Packet.WriteUInt(CharacterID);
            return true;
        }
    }
}