using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Base.Factories;
using Game.Client;
using Game.Server.Writers;
using Game.Data.Enums;
using Game.Data.Models;
using Game.Server.Manager;

namespace Game.Server.Commands
{
    public class TeleportCommand : GCommand
    {
        public override string Name
        {
            get
            {
                return "teleport";
            }
        }

        public override bool Execute(params string[] Arguments)
        {
            GameClient Client = GetParameter<GameClient>("Client");
            if ((Client == null || Client.Account.Access >= AccessLevel.Moderator) && Arguments.Length == 1)
            {
                int MapID;
                MapModel Map;

                if(int.TryParse(Arguments[0], out MapID) && (Map = WorldManager.GetMapByID(MapID)) != null)
                {
                    WorldManager.TeleportPlayer(Client, Map);

                    return true;
                }
                return false;
            }
            else return false;
        }
    }
}
