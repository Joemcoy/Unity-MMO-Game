using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiMMORPG.Server.BattleRoyale.Commands
{
    using Manager;
    using General.Requests;

    public class RoomTimeCommand : BRCommand
    {
        public override string Name => "rtime";
        public override string Description => "Show the time of room!";

        public override bool Parse(object caller, params string[] args)
        {
            if (!base.Parse(caller, args)) return false;
            if (Client != null)
            {
                if (Client.RoomID == Guid.Empty)
                {
                    Logger.LogWarning("Only room command!");
                    return false;
                }
                return true;
            }
            else
            {
                Logger.LogWarning("Only client command!");
                return false;
            }
        }

        public override bool Execute()
        {
            var room = RoomManager.GetRoomByID(Client.RoomID);
            var packet = new ChatRequest();

            if (room != null)
            {
                packet.Message = string.Format("{0}:{1}", room.Time.Hours, room.Time.Minutes);                
                Client.Socket.Send(packet);
                return true;
            }
            return false;
        }
    }
}