using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Network.Interfaces;
namespace PiMMORPG.Server.General.Requests
{
    using Client;
    using Models;

    public class MoveCharacterRequest : PiBaseRequest
    {
        public override ushort ID => PacketID.MoveCharacter;

        public uint CharacterID { get; set; }
        public Position Position { get; set; }

        public override bool Write(IDataPacket packet)
        {
            packet.WriteUInt(CharacterID);
            packet.WriteWrapper(Position);
            return true;
        }
    }
}