using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Game.Data.Models;
using Base.Data.Interfaces;
using Base.Factories;

namespace Game.Controller
{
    public class WorldItemGroupManager
    {
        public static WorldItemGroupModel GetGroupByID(int ID)
        {
            IBaseController Base = ControllerFactory.GetBaseController("world_item_group");
            return (WorldItemGroupModel)Base.GetModel(ID);
        }
    }
}