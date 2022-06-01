using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Factories;
namespace PiMMORPG.Server.RPG.Commands
{
    using Models;
    using Manager;
    using General.Commands;

    public class DropItemCommand : RPGCommand
    {
        public override string Name => "drop";
        public override string Description => "Drop an item on player world!";

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
            var drop = new Drop
            {
                Serial = Guid.NewGuid(),
                InventoryID = itemID,
                Quantity = quantity,
                Map = Client.Character.Map
            };
            drop.Copy(Client.Character.Position);
            WorldManager.DropItem(Client, drop);

            return true;
        }
    }
}