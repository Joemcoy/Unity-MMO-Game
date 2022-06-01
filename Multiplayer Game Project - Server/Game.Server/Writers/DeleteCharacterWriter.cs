using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Game.Data;
using Game.Data.Models;
using Server.Configuration;

using Network.Data;
using Network.Data.Interfaces;

namespace Game.Server.Writers
{
    public class DeleteCharacterWriter : IRequest
    {
        public byte Result { get; set; }

        public uint ID { get { return PacketID.DeleteCharacter; } }
        public bool Write(IClientSocket Client, ISocketPacket Packet)
        {
            Packet.WriteByte(Result);
            return true;
        }
    }
}
