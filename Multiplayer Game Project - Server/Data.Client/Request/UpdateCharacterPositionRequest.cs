using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Game.Data.Models;
using Network.Data.Interfaces;

namespace Data.Client.Requests
{
    public class UpdateCharacterPositionRequest : IRequest
    {
        public uint ID { get { return PacketID.DataUpdateCharacterPosition; } }
        public CharacterModel Character { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            Character.WritePacket(Packet);
            return true;
        }
    }
}
