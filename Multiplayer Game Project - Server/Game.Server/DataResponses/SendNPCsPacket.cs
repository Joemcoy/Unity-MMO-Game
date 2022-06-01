using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Network.Data;
using Network.Data.Interfaces;
using Game.Data;
using Game.Data.Models;
using Game.Client;
using Data.Client;
using Game.Server.Manager;
using Base.Factories;
using Game.Server.Writers;

namespace Game.Server.DataResponses
{
    public class SendNPCsPacket : DCResponse
    {
        NPCModel[] NPCs;
        Dictionary<int, List<NPCPositionModel>> Spawns;

        public override uint ID { get { return PacketID.DataSendNPCs; } }
        public override bool Read(ISocketPacket Packet)
        {
            NPCs = new NPCModel[Packet.ReadInt()];
            Spawns = new Dictionary<int, List<NPCPositionModel>>();

            for (int i = 0; i < NPCs.Length; i++)
            {
                NPCs[i] = new NPCModel();
                NPCs[i].ReadPacket(Packet);

                var Spawns = new NPCPositionModel[Packet.ReadInt()];
                for (int j = 0; j < Spawns.Length; j++)
                {
                    Spawns[j] = new NPCPositionModel();
                    Spawns[j].ReadPacket(Packet);
                }

                if (!this.Spawns.ContainsKey(NPCs[i].ID))
                    this.Spawns[NPCs[i].ID] = new List<NPCPositionModel>();
                this.Spawns[NPCs[i].ID].AddRange(Spawns);
            }

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            try
            {
                WorldManager.RegisterNPCs(NPCs, Spawns);

                var Server = SingletonFactory.GetInstance<GameServer>();
                Server.Listen();
            }
            catch (Exception ex)
            {
                LoggerFactory.GetLogger(this).LogFatal(ex);
                SingletonFactory.Destroy<GameServer>();
            }
        }
    }
}