using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tFramework.Network.Interfaces;

namespace PiMMORPG.Server.RPG.Responses
{
    using Manager;
    using Client.RPG;

    public class RemoveItemResponse : General.Responses.RemoveItemResponse
    {
        public override void Execute()
        {
            ItemManager.RemoveItem(Client as PiRPGClient, serial);
        }
    }
}