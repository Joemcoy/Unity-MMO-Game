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
    public class SendTreesPacket : DCResponse
    {
        TreeModel[] Trees;
        Dictionary<int, List<TreePositionModel>> Spawns;

        public override uint ID { get { return PacketID.DataSendTrees; } }
        public override bool Read(ISocketPacket Packet)
        {
            Trees = new TreeModel[Packet.ReadInt()];
            Spawns = new Dictionary<int, List<TreePositionModel>>();

            for (int i = 0; i < Trees.Length; i++)
            {
                Trees[i] = new TreeModel();
                Trees[i].ReadPacket(Packet);

                var MobSpawns = new TreePositionModel[Packet.ReadInt()];
                for (int j = 0; j < MobSpawns.Length; j++)
                {
                    MobSpawns[j] = new TreePositionModel();
                    MobSpawns[j].ReadPacket(Packet);
                }

                if (!this.Spawns.ContainsKey(Trees[i].ID))
                    this.Spawns[Trees[i].ID] = new List<TreePositionModel>();
                this.Spawns[Trees[i].ID].AddRange(MobSpawns);
            }

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            try
            {
                WorldManager.RegisterTrees(Trees, Spawns);

                var Server = SingletonFactory.GetInstance<GameServer>().Socket;
                Client.SendNPCs(Server.EndPoint.Port);
            }
            catch (Exception ex)
            {
                LoggerFactory.GetLogger(this).LogFatal(ex);
                SingletonFactory.Destroy<GameServer>();
            }
        }
    }
}