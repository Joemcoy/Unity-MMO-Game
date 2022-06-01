using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Network.Data.Interfaces;

namespace Data.Server.Writers
{
    public class DeleteCharacterRequest : IRequest
    {
        public uint ID { get { return PacketID.DataDeleteCharacter; } }
        public byte Result { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            Packet.WriteByte(Result);

            return true;
        }
    }
}
