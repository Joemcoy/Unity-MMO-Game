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
    public class SpawnPlayerWriter : IPacketWriter
    {
        public CharacterModel Character { get; set; }

        public uint ID { get { return PacketID.SpawnPlayer; } }
        public bool Write(IClientSocket Client, SocketPacket Packet)
        {
            Character.WritePacket(Packet);
            return true;
        }
    }
}