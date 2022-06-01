using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Network.Data.Interfaces;
using Game.Client;
using Game.Data;
using Game.Data.Models;
using Game.Server.Writers;
using Game.Server.Manager;

namespace Game.Server.Responses
{
    public class SpawnMobPacket : GCResponse
    {
        public override uint ID { get { return PacketID.SpawnMob; } }
        public override bool Read(ISocketPacket Packet)
        {
            return true;
        }

        public override void Execute(IClientSocket Socket)
        {

            var MPacket = new SpawnMobWriter();
            var Mobs = WorldManager.GetMobSpawns(Client.CurrentMap.ID);
            
            for (int i = 0; i < Mobs.Length + 1; i++)
            {
                MPacket.End = i == Mobs.Length;
                if (!MPacket.End)
                {
                    MPacket.Spawn = Mobs[i];
                    MPacket.Mob = WorldManager.GetMobByID(MPacket.Spawn.MobID);
                }
                Socket.Send(MPacket);
            }
        }
    }
}
