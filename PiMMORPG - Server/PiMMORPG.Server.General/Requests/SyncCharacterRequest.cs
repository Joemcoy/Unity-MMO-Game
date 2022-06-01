using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tFramework.Network.Interfaces;

namespace PiMMORPG.Server.General.Requests
{
    using Client;
    using Models;

    public class SyncCharacterRequest : PiBaseRequest
    {
        public override ushort ID => PacketID.SyncCharacter;

        public uint CharacterId { get; set; }
        public Position Position { get; set; }
        public float Horizontal { get; set; }
        public float Vertical { get; set; }

        public override bool Write(IDataPacket packet)
        {
            packet.WriteUInt(CharacterId);
            Position.WritePacket(packet);
            packet.WriteFloat(Horizontal);
            packet.WriteFloat(Vertical);
            return true;
        }
    }
}
