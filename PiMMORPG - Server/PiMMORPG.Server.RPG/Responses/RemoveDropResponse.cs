using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiMMORPG.Server.RPG.Responses
{
    using Manager;
    using Client.RPG;

    public class RemoveDropResponse : General.Responses.RemoveDropResponse
    {
        public override void Execute()
        {
            base.Execute();
            ItemManager.AddItem(Client as PiRPGClient, drop.InventoryID, drop.Serial, drop.Quantity);
        }
    }
}