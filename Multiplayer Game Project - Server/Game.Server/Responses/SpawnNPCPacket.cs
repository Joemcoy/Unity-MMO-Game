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
    public class SpawnNPCPacket : GCResponse
    {
        public override uint ID { get { return PacketID.SpawnNPC; } }
        public override bool Read(ISocketPacket Packet)
        {
            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            var NPacket = new SpawnNPCWriter();
            var NPCs = WorldManager.GetNPCSpawns(Client.CurrentMap.ID);

            for(int i = 0; i <= NPCs.Length; i++)
            {
                NPacket.End = i == NPCs.Length;

                if (!NPacket.End)
                {
                    NPacket.Spawn = NPCs[i];
                    NPacket.NPC = WorldManager.GetNPCByID(NPacket.Spawn.NPC);
                }
                Socket.Send(NPacket);
            }
        }
    }
}
