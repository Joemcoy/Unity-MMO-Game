using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Game.Data.Models;
using Network.Data.Interfaces;

namespace Game.Server.Writers
{
    public class SpawnMobWriter : IRequest
    {
        public uint ID { get { return PacketID.SpawnMob; } }

        public bool End { get; set; }
        public MobModel Mob { get; set; }
        public MobPositionModel Spawn { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            Packet.WriteBool(End);

            if (!End)
            {
                Mob.WritePacket(Packet);
                Spawn.WritePacket(Packet);
            }

            return true;
        }
    }
}
