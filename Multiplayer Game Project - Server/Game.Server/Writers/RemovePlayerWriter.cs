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
    public class RemovePlayerWriter : IRequest
    {
        public CharacterModel Character { get; set; }

        public uint ID { get { return PacketID.RemovePlayer; } }
        public bool Write(IClientSocket Client, ISocketPacket Packet)
        {
            Packet.WriteInt(Character.ID);
            return true;
        }
    }
}
