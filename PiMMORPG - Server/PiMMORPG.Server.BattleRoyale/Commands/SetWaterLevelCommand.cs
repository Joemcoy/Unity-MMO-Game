﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiMMORPG.Server.BattleRoyale.Commands
{
    using Manager;

    public class SetWaterLevel : BRCommand
    {
        public override string Name => "wlevel";
        public override string Description => "Set the water level room!";

        int level;
        public override bool Parse(object caller, params string[] args)
        {
            if (!base.Parse(caller, args)) return false;
            else if (caller == null)
            {
                Logger.LogWarning("Only client command!");
                return false;
            }
            else if (Client.RoomID == Guid.Empty)
            {
                Logger.LogWarning("Only inside room command!");
                return false;
            }
            else if (args.Length == 1)
            {
                if (!int.TryParse(args[0], out level))
                    return false;
                else
                    return true;
            }
            else return false;
        }

        public override bool Execute()
        {
            var room = RoomManager.GetRoomByID(Client.RoomID);
            if (room == null)
            {
                Logger.LogWarning("Client room not found!");
                return false;
            }
            else
            {
                room.SetWaterLevel(level);
                return true;
            }
        }
    }
}
