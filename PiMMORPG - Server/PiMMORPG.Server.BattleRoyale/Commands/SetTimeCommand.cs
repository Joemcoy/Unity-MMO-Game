using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiMMORPG.Server.BattleRoyale.Commands
{
    using Manager;

    public class SetTimeCommand : BRCommand
    {
        public override string Name => "settime";
        public override string Description => "Set the time of current room!";

        int hours, minutes;
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
            else if (args.Length == 2)
            {
                if (!int.TryParse(args[0], out hours))
                    return false;
                else if (!int.TryParse(args[1], out minutes))
                    return false;
                else
                    return true;
            }
            else return false;
        }

        public override bool Execute()
        {
            var room = RoomManager.GetRoomByID(Client.RoomID);
            if(room == null)
            {
                Logger.LogWarning("Client room not found!");
                return false;
            }
            else
            {
                room.SetTime(hours, minutes);
                return true;
            }
        }
    }
}
