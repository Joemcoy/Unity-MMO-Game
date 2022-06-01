using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Base.Data.Interfaces;
using Game.Data.Models;
using Base.Factories;

namespace Game.Controller
{
    public class WorldItemManager : ISingleton
    {
        IBaseController WorldItems;
        void ISingleton.Create()
        {
            WorldItems = ControllerFactory.GetBaseController("world_items");

            WorldItems.RegisterAfterLoadModelCallback(LoadPositions);
            WorldItems.RegisterAfterSaveModelCallback(SavePositions);
        }

        void ISingleton.Destroy() { }

        private void LoadPositions(IBaseController Controller)
        {
            foreach(WorldItemModel Model in Controller.GetModels<WorldItemModel>())
            {
                Model.Item = ItemManager.GetItemByID(Model.ItemID);
                Model.Position = WorldItemPositionManager.GetPositionByID(Model.ID);
                Model.Group = WorldItemGroupManager.GetGroupByID(Model.GroupID);
            }
        }

        private void SavePositions(IBaseController Controller, IModel Model)
        {
            IBaseController PositionController = ControllerFactory.GetBaseController("world_item_position");
            PositionController.SaveModel(((WorldItemModel)Model).Position);

            IBaseController GroupController = ControllerFactory.GetBaseController("world_item_group");
            IModel GroupModel = GroupController.SaveModel(((WorldItemModel)Model).Group);
        }

        public static WorldItemModel[] GetItemsByMapID(int MapID)
        {
            var Manager = SingletonFactory.GetInstance<WorldItemManager>();
            return Manager.WorldItems.GetModels<WorldItemModel>(M => M.Position.MapID == MapID);
        }

        public static WorldItemModel[] GetAllItems()
        {
            var Manager = SingletonFactory.GetInstance<WorldItemManager>();
            return Manager.WorldItems.GetModels<WorldItemModel>();
        }

        public static void AddItem(WorldItemModel Item)
        {
            var Manager = SingletonFactory.GetInstance<WorldItemManager>();
            Manager.WorldItems.AddModel(Item);
        }
    }
}
