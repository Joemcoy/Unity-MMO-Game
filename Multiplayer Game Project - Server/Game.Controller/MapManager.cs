using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data.Models;
using Base.Data.Interfaces;
using Base.Factories;

namespace Game.Controller
{
    public class MapManager : ISingleton
    {
        IBaseController Maps;

        void ISingleton.Create()
        {
            Maps = ControllerFactory.GetBaseController("maps");
            Maps.RegisterAfterLoadModelCallback(LoadSpawns);
        }

        void ISingleton.Destroy() { }

        private void LoadSpawns(IBaseController Controller)
        {
            foreach (MapModel Map in Controller.GetModels().Cast<MapModel>())
            {
                Map.Spawn = SpawnManager.GetSpawnByID(Map.SpawnID);
            }
        }

        public static MapModel GetMapByID(int ID)
        {
            var Manager = SingletonFactory.GetInstance<MapManager>();
            return Manager.Maps.GetModel<MapModel>(ID);
        }

        public static MapModel[] GetMaps()
        {
            var Manager = SingletonFactory.GetInstance<MapManager>();
            return Manager.Maps.GetModels<MapModel>();
        }
    }
}