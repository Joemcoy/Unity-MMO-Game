using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data.Models;
using Base.Data.Interfaces;
using Base.Factories;

namespace Game.Controller
{
    public class SpawnManager
    {
        public static PositionModel GetSpawnByID(int SpawnID)
        {
            IBaseController Base = ControllerFactory.GetBaseController("map_spawns");
            return (PositionModel)Base.GetModel(SpawnID);
        }
    }
}