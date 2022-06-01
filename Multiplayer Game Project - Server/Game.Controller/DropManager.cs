using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Base.Factories;
using Base.Data.Interfaces;
using Game.Data.Models;

namespace Game.Controller
{
    public class DropManager : ISingleton
    {
        IBaseController Drops, Positions;

        void ISingleton.Create()
        {
            Drops = ControllerFactory.GetBaseController("drops");
            Positions = ControllerFactory.GetBaseController("drop_position");

            Drops.RegisterAfterLoadModelCallback(LoadDropPosition);
            Drops.RegisterAfterSaveModelCallback(LoadDropPosition);
        }

        void ISingleton.Destroy() { }

        private void LoadDropPosition(IBaseController Base)
        {
            foreach (var Drop in Drops.GetModels<DropModel>())
                Drop.Position = Positions.GetModel<PositionModel>(Drop.ID);
        }

        private void LoadDropPosition(IBaseController Base, IModel Model)
        {
            var Drop = Model as DropModel;
            Drop.Position.ID = Drop.ID;

            Positions.UpdateModel(Drop.Position);
        }
    }
}
