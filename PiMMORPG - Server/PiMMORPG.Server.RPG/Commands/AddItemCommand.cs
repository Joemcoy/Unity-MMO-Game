using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Factories;
namespace PiMMORPG.Server.RPG.Commands
{
    using Manager;
    using Client.RPG;
    using General.Commands;

    public class AddItemCommand : RPGCommand
    {
        public override string Name => "additem";
        public override string Description => "Add item to player!";

        uint itemID, quantity;
        public override bool Parse(object caller, params string[] args)
        {
            base.Parse(caller, args);
            if (Client == null)
                LoggerFactory.GetLogger(this).LogError("Only client command!");
            else if(args.Length >= 1)
            {
                itemID = Convert.ToUInt32(args[0]);
                quantity = args.Length == 2 ? Convert.ToUInt32(args[1]) : 1;
                return true;
            }
            return false;
        }

        public override bool Execute()
        {
            ItemManager.AddItem(Client, itemID, Guid.NewGuid(), quantity);
            return true;
        }
    }
}