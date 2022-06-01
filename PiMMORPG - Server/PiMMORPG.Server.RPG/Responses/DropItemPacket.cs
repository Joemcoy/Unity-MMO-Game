using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Factories;
using tFramework.Network.Interfaces;
namespace PiMMORPG.Server.RPG.Responses
{
    using Client.RPG;
    using Models;
    using Manager;

    public class DropItemPacket : General.Responses.DropItemPacket
    {
        public override void Execute()
        {
            WorldManager.DropItem(Client as PiRPGClient, drop);   
        }
    }
}