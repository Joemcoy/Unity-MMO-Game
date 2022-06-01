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
    public class TreeManager : ASingleton<TreeManager>
    {
        IBaseController Trees;
        IBaseController TreePositions;

        protected override void Created()
        {
            Trees = ControllerFactory.GetBaseController("trees");
            TreePositions = ControllerFactory.GetBaseController("tree_spawns");
        }

        public static TreeModel[] GetAllTrees()
        {
            return Instance.Trees.GetModels<TreeModel>();
        }

        public static TreePositionModel[] GetSpawnsByTree(int TreeID, int Server)
        {
            return Instance.TreePositions.GetModels<TreePositionModel>(M => M.TreeID == TreeID && M.Server == Server);
        }
    }
}