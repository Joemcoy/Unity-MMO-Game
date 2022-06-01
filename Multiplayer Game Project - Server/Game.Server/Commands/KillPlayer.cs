using Base.Factories;
using Game.Server.Writers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Client;

namespace Game.Server.Commands
{
    public class KillPlayer : GCommand
    {
        public override string Name { get { return "kill"; } }

        public override bool Execute(params string[] Arguments)
        {
            if(Arguments.Length > 0)
            {
                var Server = SingletonFactory.GetInstance<GameServer>();
                var SenderClient = GetParameter<GameClient>("Client");
                var Sender = SenderClient == null ? "Server" : SenderClient.CurrentCharacter.Name;

                if (Server.Clients.Any(C => C.CurrentCharacter != null && C.CurrentCharacter.Name == Arguments[0]))
                {
                    var Packet = new KillPlayerWriter();
                    Packet.Enemy = Sender;
                    Packet.Target = Server.Clients.First(C => C.CurrentCharacter.Name == Arguments[0]).CurrentCharacter.Name;
                    Packet.Level = Sender == "Server" ? 1 : SenderClient.CurrentCharacter.Stats.Level;
                    Packet.Experience = Sender == "Server" ? 0 : SenderClient.CurrentCharacter.Stats.Experience;

                    foreach (var Client in Server.Clients)
                    {
                        Client.Socket.Send(Packet);
                    }

                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
    }
}
