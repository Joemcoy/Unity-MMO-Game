using Base.Data.Interfaces;
using Base.Factories;
using Game.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Data.Enums;

namespace Game.Controller
{
    public class CharacterItemManager
    {
        public static CharacterItemModel GetStatsByID(int ID)
        {
            IBaseController Base = ControllerFactory.GetBaseController("character_items");
            return Base.GetModel<CharacterItemModel>(ID);
        }

        public static CharacterItemModel[] GetItemsByOwner(int OwnerID)
        {
            IBaseController Base = ControllerFactory.GetBaseController("character_items");
            return Base.GetModels<CharacterItemModel>(I => I.OwnerID == OwnerID);
        }

        public static CharacterItemModel[] GetEquipsByOwner(int OwnerID)
        {
            IBaseController Base = ControllerFactory.GetBaseController("character_items");
            IBaseController ItemsBase = ControllerFactory.GetBaseController("items");

            return Base.GetModels<CharacterItemModel>(I =>
            {
                var RealItem = ItemsBase.GetModel<ItemModel>(I.ItemID);
                return I.OwnerID == OwnerID && I.Equiped && RealItem.CanEquip() && RealItem.Type != ItemType.ActionScroll && RealItem.Type != ItemType.Scroll;
            });
        }

        public static void UpdateItem(CharacterItemModel Item)
        {
            IBaseController Base = ControllerFactory.GetBaseController("character_items");
            var Model =Base.GetModels<CharacterItemModel>(I => Item.ID == -1 ? I.Serial == Item.Serial : I.OwnerID == Item.OwnerID && I.ItemID == Item.ItemID).FirstOrDefault();

            if (Model == null)
            {
                Model = new CharacterItemModel();
                Item.CopyTo(Model);

                Base.AddModel(Model);
            }
            else
            {
                Item.ID = Model.ID;
                Base.UpdateModel(Item);
            }
        }

        public static void RemoveItemBySerial(Guid Serial)
        {
            IBaseController Base = ControllerFactory.GetBaseController("character_items");
            var Model = Base.GetModel<CharacterItemModel>(C => C.Serial == Serial);

            if (Serial != null)
                Base.RemoveModel(Model);
        }

        public static void RemoveItem(CharacterItemModel Item)
        {
            IBaseController Base = ControllerFactory.GetBaseController("character_items");
            Base.RemoveModel(Item);
        }
    }
}