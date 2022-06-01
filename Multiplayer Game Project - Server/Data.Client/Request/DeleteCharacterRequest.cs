using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Network.Data.Interfaces;

namespace Data.Client.Requests
{
    public class DeleteCharacterRequest : IRequest
    {
        public uint ID { get { return PacketID.DataDeleteCharacter; } }
        public int CharacterID { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            Packet.WriteInt(CharacterID);
            return true;
        }
    }
}
