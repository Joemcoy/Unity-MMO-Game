using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Extensions;
namespace PiMMORPG.Server.BattleRoyale.Responses
{
    using Manager;
    using General.Requests;
    using Client.BattleRoyale;

    public class RequestMapDataResponse : General.Responses.RequestMapDataResponse
    {
        public override void Execute()
        {
            var room = RoomManager.GetFreeRoom(Client as PiBRClient);
            var clients = room.Clients.Where(c => c.Character.ID != Client.Character.ID);
            packet.Characters = clients.Select(c => c.Character).ToArray();

            base.Execute();
        }
    }
}