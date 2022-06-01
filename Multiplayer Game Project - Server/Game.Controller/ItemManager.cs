using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



using Game.Data.Models;
using Base.Data.Interfaces;
using Base.Factories;

namespace Game.Controller
{
    public class ItemManager
    { 
        public static ItemModel GetItemByID(int ID)
        {
            IBaseController Base = ControllerFactory.GetBaseController("items");
            return (ItemModel)Base.GetModel(ID);
        }

        public static ItemModel[] GetAllItems()
        {
            IBaseController Base = ControllerFactory.GetBaseController("items");
            return Base.GetModels().Cast<ItemModel>().ToArray();
        }
    }
}
