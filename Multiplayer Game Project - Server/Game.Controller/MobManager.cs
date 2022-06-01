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
    public class MobManager : ASingleton<MobManager>
    {
        IBaseController Mobs;
        IBaseController MobsPositions;

        protected override void Created()
        {
            Mobs = ControllerFactory.GetBaseController("mobs");
            MobsPositions = ControllerFactory.GetBaseController("mob_spawns");
        }

        public static MobModel[] GetAllMobs()
        {
            return Instance.Mobs.GetModels<MobModel>();
        }

        public static MobPositionModel[] GetSpawnsByMob(int MobID, int Server)
        {
            return Instance.MobsPositions.GetModels<MobPositionModel>(M => M.MobID == MobID && M.Server == Server);
        }
    }
}