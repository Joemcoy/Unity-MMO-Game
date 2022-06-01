using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Network.Data.Interfaces;

namespace Data.Server.Writers
{
    public class CreateCharacterRequest : IRequest
    {
        public byte Result { get; set; }

        public uint ID { get { return PacketID.DataCreateCharacter; } }
        public bool Write(IClientSocket Client, ISocketPacket Packet)
        {
            Packet.WriteByte(Result);
            return true;
        }
    }
}
