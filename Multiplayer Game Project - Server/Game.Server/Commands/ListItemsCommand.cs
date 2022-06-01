using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Server.Commands
{
    using Data.Models;
    using Base.Factories;
    using Manager;

    public class ListItemsCommand : GCommand
    {
        public override string Name => "li";

        public override bool Execute(params string[] Arguments)
        {
            string Base = string.Empty;
            var Manager = SingletonFactory.GetInstance<ItemCacheManager>();

            foreach (ItemModel Item in Manager.GetItems())
                Base += $"- ID:{Item.ID} RealID:{Item.UniqueID} Type:{Item.Type}" + Environment.NewLine;

            ChatManager.SendToClient(Client, new MessageModel { Access = Data.Enums.AccessLevel.Server, Content = Base });
            return true;
        }
    }
}