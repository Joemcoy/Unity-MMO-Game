using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base.Factories;
using Game.Data.Models;

namespace Game.Controller
{
    public class CharacterStartItemsManager
    {
        public static CharacterStartItemModel[] GetItemsByClass(int Class)
        {
            var Base = ControllerFactory.GetBaseController("character_start_items");
            return Base.GetModels<CharacterStartItemModel>(I => I.Class == Class);
        }
    }
}
