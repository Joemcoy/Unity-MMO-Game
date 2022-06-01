using Base.Factories;
using Game.Client;
using Gate.Client.Responses.Writers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Data.Models;
using Game.Data.Enums;
using Game.Server.Manager;
using Game.Server.Writers;

namespace Game.Server.Commands
{
    public class GoToCommand : GCommand
    {
        public override string Name
        {
            get
            {
                return "goto";
            }
        }

        public override bool Execute(params string[] Arguments)
        {
            GameClient Client = GetParameter<GameClient>("Client");
            if (Client != null && Client.Account.Access == AccessLevel.Administrator && Arguments.Length >= 1)
            {
                var Server = SingletonFactory.GetInstance<GameServer>();
                var Manager = SingletonFactory.GetInstance<ItemCacheManager>();

                var Message = new MessageModel();
                Message.Type = MessageType.System;
                Message.Access = AccessLevel.Server;
                
                GameClient ToClient = Client, Target = null;
                if(Arguments.Length == 2)
                {
                    ToClient = Server.Clients.FirstOrDefault(C => C.CurrentCharacter != null && C.CurrentCharacter.Name == Arguments[0]);
                    Target = Server.Clients.FirstOrDefault(C => C.CurrentCharacter != null && C.CurrentCharacter.Name == Arguments[1]);
                }
                else
                    Target = ToClient = Server.Clients.FirstOrDefault(C => C.CurrentCharacter != null && C.CurrentCharacter.Name == Arguments[0]);

                ToClient.CurrentCharacter.Position.CopyTo(Target.CurrentCharacter.Position);

                var Packet = new SetPlayerPositionWriter();
                Packet.CharacterID = Target.CurrentCharacter.ID;
                Packet.Position = Target.CurrentCharacter.Position;

                foreach (var Remote in WorldManager.GetPlayersInMap(Target.CurrentMap.ID))
                    Remote.Socket.Send(Packet);

                return true;
            }
            else return false;
        }
    }
}