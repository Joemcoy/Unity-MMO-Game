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

namespace Game.Server.Commands
{
    public class PrivateMessageCommand : GCommand
    {
        public override string Name
        {
            get
            {
                return "pm";
            }
        }

        public override bool Execute(params string[] Arguments)
        {
            GameClient Client = GetParameter<GameClient>("Client");
            if (Client != null && Arguments.Length > 1)
            {
                var Server = SingletonFactory.GetInstance<GameServer>();
                var Line = string.Join(" ", Arguments);

                var reg = new Regex("\".*?\"");
                var matches = reg.Matches(Line);

                string Target = Arguments[0];

                if (matches.Count > 1)
                {
                    Target = matches[0].ToString();
                    Target = Target.Substring(2, Target.Length - 2);
                }
                LoggerFactory.GetLogger(this).LogInfo(Target);

                var Message = string.Join(" ", Arguments).Substring(Target.Length);
                var TargetClient = Server.Clients.FirstOrDefault(C => C.CurrentCharacter != null && C.CurrentCharacter.Name == Target);
                var Packet = new PrivateMessageWriter();

                if (TargetClient != null)
                {
                    if (TargetClient.CurrentCharacter.Name == Client.CurrentCharacter.Name)
                    {
                        Packet.Result = PMResult.SameUser;
                        Client.Socket.Send(Packet);
                    }
                    else
                    {
                        Packet.Result = PMResult.Sent;
                        Packet.Message = new MessageModel();
                        Packet.Message.Username = Client.CurrentCharacter.Name;
                        Packet.Message.Access = Client.Account.Access;
                        Packet.Message.Content = Message;
                        Packet.Message.SentTime = DateTime.Now;
                        Packet.Message.Type = MessageType.Private;
                        TargetClient.Socket.Send(Packet);
                    }
                }
                else
                {
                    Packet.Result = PMResult.Offline;
                    Client.Socket.Send(Packet);
                }

                return true;
            }
            else return false;
        }
    }
}