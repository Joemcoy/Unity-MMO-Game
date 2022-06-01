using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Factories;
using tFramework.Extensions;

namespace PiMMORPG.Server.BattleRoyale.Responses
{
    using Manager;
    using Client.Interfaces;
    using Client.BattleRoyale;

    using Server.General;
    using Server.General.Requests;

    public class ChatResponse : General.Responses.ChatResponse
    {
        bool Validate(IGameClient client)
        {
            return client.Character.ID == Client.Character.ID;
        }

        public override void Execute()
        {
            var server = ServerControl.GetServer(Client.Socket.Server.EndPoint.Port);

            if (message.StartsWith("/"))
            {
                if (!CommandFactory.ExecuteCommand(message.Substring(1), Client))
                {
                    server.SendSystemMessage("Not valid command!", Validate);
                }
            }
            else
            {
                var packet = new ChatRequest
                { Message = string.Format("<b><color={0}>{1}</color></b>: {2}", Client.Account.Access.LevelColor, Client.Character.Name, message) };

                var brcl = Client as PiBRClient;
                if (brcl.RoomID != Guid.Empty)
                {
                    var room = RoomManager.GetRoomByID(brcl.RoomID);
                    room.Clients.ForEach(c => c.Socket.Send(packet));
                }
            }
        }
    }
}