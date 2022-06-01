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
    public class SendNPCsRequest : IRequest
    {
        public uint ID { get { return PacketID.DataSendNPCs; } }
        public int Server { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            var NPCs = NPCManager.GetAllNPCs();

            Packet.WriteInt(NPCs.Length);
            foreach (var NPC in NPCs)
            {
                NPC.WritePacket(Packet);

                var Spawns = NPCManager.GetSpawnsByMob(NPC.ID, Server);

                Packet.WriteInt(Spawns.Length);
                foreach (var Spawn in Spawns)
                    Spawn.WritePacket(Packet);
            }

            return true;
        }
    }
}
