using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Controller;
using Game.Data;
using Game.Data.Models;
using Game.Data.Results;
using Network.Data.Interfaces;

namespace Data.Server.Writers
{
    public class SendMobsRequest : IRequest
    {
        public uint ID { get { return PacketID.DataSendMobs; } }
        public int Server { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            var Mobs = MobManager.GetAllMobs();

            Packet.WriteInt(Mobs.Length);
            foreach(var Mob in Mobs)
            {
                Mob.WritePacket(Packet);

                var Spawns = MobManager.GetSpawnsByMob(Mob.ID, Server);

                Packet.WriteInt(Spawns.Length);
                foreach (var Spawn in Spawns)
                    Spawn.WritePacket(Packet);
            }

            return true;
        }
    }
}
