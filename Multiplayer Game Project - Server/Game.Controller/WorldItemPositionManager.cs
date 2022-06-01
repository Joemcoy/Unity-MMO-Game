using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Game.Data.Models;
using Base.Data.Interfaces;
using Base.Factories;

namespace Game.Controller
{
    public class WorldItemPositionManager
    { 
        public static PositionModel GetPositionByID(int ID)
        {
            IBaseController Base = ControllerFactory.GetBaseController("world_item_position");
            return (PositionModel)Base.GetModel(ID);
        }
    }
}