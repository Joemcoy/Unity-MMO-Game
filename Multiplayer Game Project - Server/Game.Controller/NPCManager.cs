using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base.Data.Abstracts;
using Base.Data.Interfaces;
using Base.Factories;

using Game.Data.Models;

namespace Game.Controller
{
    public class NPCManager : ASingleton<NPCManager>
    {
        IBaseController NPCs;
        IBaseController Spawns;

        protected override void Created()
        {
            NPCs = ControllerFactory.GetBaseController("npcs");
            Spawns = ControllerFactory.GetBaseController("npc_spawns");
        }

        public static NPCModel[] GetAllNPCs()
        {
            return Instance.NPCs.GetModels<NPCModel>();
        }

        public static NPCPositionModel[] GetSpawnsByMob(int NPC, int Server)
        {
            return Instance.Spawns.GetModels<NPCPositionModel>(M => M.NPC == NPC && M.Server == Server);
        }
    }
}