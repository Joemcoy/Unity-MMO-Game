using Base.Factories;
using Game.Client;
using Gate.Client.Responses.Writers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text.RegularExpressions;
using Game.Server.Writers;
using Game.Data.Models;
using Game.Data.Results;
using Game.Data.Enums;
using Game.Server.Manager;

namespace Game.Server.Commands
{
    public class PingCommand : GCommand
    {
        public override string Name
        {
            get
            {
                return "ping";
            }
        }

        public override bool Execute(params string[] Arguments)
        {
            GameClient Client = GetParameter<GameClient>("Client");
            if (Client != null)
            {
                var Message = new MessageModel();
                Message.Content = "LM:Messages.Pong";
                Message.Type = MessageType.System;
                Message.Access = AccessLevel.Server;

                ChatManager.SendToClient(Client, Message, Client.Socket.Ping);
                return true;
            }
            else return false;
        }
    }
}