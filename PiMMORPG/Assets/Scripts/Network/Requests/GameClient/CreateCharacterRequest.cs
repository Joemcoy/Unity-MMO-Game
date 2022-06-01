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
    public class CreateCharacterRequest : PiBaseRequest
    {
        public override ushort ID { get { return PacketID.CreateCharacter; } }
        public string Name { get; set; }
        public bool IsFemale { get; set; }
        public CharacterStyle Style { get; set; }

        public override bool Write(IDataPacket packet)
        {
            packet.WriteString(Name);
            packet.WriteBool(IsFemale);
            Style.WritePacket(packet);
            return true;
        }
    }
}