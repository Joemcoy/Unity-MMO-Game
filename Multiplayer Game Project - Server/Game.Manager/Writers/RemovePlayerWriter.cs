using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Game.Data;
using Game.Data.Models;

using Socket.Data;
using Socket.Data.Interfaces;

namespace Game.Manager.Writers
{
    public class RemovePlayerWriter : IPacketWriter
    {
        public CharacterModel Character { get; set; }

        public uint ID { get { return PacketID.RemovePlayer; } }
        public bool Write(IClientSocket Client, SocketPacket Packet)
        {
            Packet.WriteString(Character.Name);
            return true;
        }
    }
}
