using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiMMORPG.Server.BattleRoyale.Commands
{
    using Manager;
    using General.Requests;

    public class RoomInfoCommand : BRCommand
    {
        public override string Name => "rinfo";
        public override string Description => "Show the information of room!";

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
                packet.Message = string.Format("Room ID: {0}", room.ID) + Environment.NewLine;
                packet.Message += string.Format("Room state: {0}", room.State) + Environment.NewLine;
                packet.Message += string.Format("Room time: {0}:{1}", room.Time.Hours, room.Time.Minutes) + Environment.NewLine;
                packet.Message += string.Format("Water level: {0}", room.WaterLevel) + Environment.NewLine;
                packet.Message += string.Format("Game seconds per real second: {0}", room.Add) + Environment.NewLine;
                packet.Message += string.Format("Client count: {0}", room.Clients.Length);
                
                Client.Socket.Send(packet);
                return true;
            }
            return false;
        }
    }
}