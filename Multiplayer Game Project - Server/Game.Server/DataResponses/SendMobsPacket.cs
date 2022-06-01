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
    public class SendMobsPacket : DCResponse
    {
        MobModel[] Mobs;
        Dictionary<int, List<MobPositionModel>> Spawns;

        public override uint ID { get { return PacketID.DataSendMobs; } }
        public override bool Read(ISocketPacket Packet)
        {
            Mobs = new MobModel[Packet.ReadInt()];
            Spawns = new Dictionary<int, List<MobPositionModel>>();

            for (int i = 0; i < Mobs.Length; i++)
            {
                Mobs[i] = new MobModel();
                Mobs[i].ReadPacket(Packet);

                var MobSpawns = new MobPositionModel[Packet.ReadInt()];
                for(int j = 0; j < MobSpawns.Length; j++)
                {
                    MobSpawns[j] = new MobPositionModel();
                    MobSpawns[j].ReadPacket(Packet);
                }

                if (!this.Spawns.ContainsKey(Mobs[i].ID))
                    this.Spawns[Mobs[i].ID] = new List<MobPositionModel>();
                this.Spawns[Mobs[i].ID].AddRange(MobSpawns);
            }

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            try
            {
                WorldManager.RegisterMobs(Mobs, Spawns);

                var Server = SingletonFactory.GetInstance<GameServer>().Socket;
                Client.SendTrees(Server.EndPoint.Port);
            }
            catch (Exception ex)
            {
                LoggerFactory.GetLogger(this).LogFatal(ex);
                SingletonFactory.Destroy<GameServer>();
            }
        }
    }
}