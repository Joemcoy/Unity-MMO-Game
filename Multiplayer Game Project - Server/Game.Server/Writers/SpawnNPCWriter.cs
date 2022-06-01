using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Game.Data.Models;
using Network.Data.Interfaces;
using Game.Server.Manager;

namespace Game.Server.Writers
{
    public class SpawnNPCWriter : IRequest
    {
        public uint ID { get { return PacketID.SpawnNPC; } }

        public bool End { get; set; }
        public NPCModel NPC { get; set; }
        public NPCPositionModel Spawn { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            Packet.WriteBool(End);

            if (!End)
            {
                NPC.WritePacket(Packet);
                Spawn.WritePacket(Packet);

                if (Spawn.HasDialogue)
                {
                    string Dialogue = WorldManager.GetNPCDialogue(Spawn.ID);
                    Packet.WriteString(Dialogue);
                }
            }
            return true;
        }
    }
}
