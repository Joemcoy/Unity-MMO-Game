using Base.Factories;
using Game.Client;
using Gate.Client.Responses.Writers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Data.Models;
using Game.Server.Manager;

namespace Game.Server.Commands
{
    public class SystemCommand : GCommand
    {
        public override string Name
        {
            get
            {
                return "system";
            }
        }

        public override bool Execute(params string[] Arguments)
        {
            GameClient Client = GetParameter<GameClient>("Client");
            if ((Client == null || Client.Account.Access == Data.Enums.AccessLevel.Administrator) && Arguments.Length > 0)
            {
                var Message = new MessageModel();
                Message.Username = Client == null ? "SERVER" : Client.CurrentCharacter.Name;
                Message.Content = string.Join(" ", Arguments);
                Message.Type = Data.Enums.MessageType.System;

                ChatManager.SendToAll(Message);
                return true;
            }
            else return false;
        }
    }
}