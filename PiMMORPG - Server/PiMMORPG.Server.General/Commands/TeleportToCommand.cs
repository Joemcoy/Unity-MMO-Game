using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiMMORPG.Server.General.Commands
{
    using Requests;
    public class TeleportToCommand : BaseCommand
    {
        public override string Name => "teleport";
        public override string Description => "Teleport an player to another";

        string from, to;

        public override bool Parse(object caller, params string[] args)
        {
            if (!base.Parse(caller, args))
                return false;
            else if(Client == null)
            {
                Logger.LogWarning("Only client command!");
                return false;
            }
            else if (args.Length != 2)
            {
                Logger.LogWarning("The teleport command need an two nicknames!");
                return false;
            }
            else
            {
                from = args[0];
                to = args[1];
                return true;
            }
        }

        public override bool Execute()
        {
            var server = ServerControl.GetServer(Client.Socket.Server.EndPoint.Port);
            if (server != null)
            {
                var from = server.Clients.FirstOrDefault(c => c.Character != null && c.Character.Name.ToLower() == this.from.ToLower());
                var to = server.Clients.FirstOrDefault(c => c.Character != null && c.Character.Name.ToLower() == this.to.ToLower());

                if (from == null) Logger.LogError("Player {0} has not been found!", this.from);
                else if(to == null) Logger.LogError("Player {0} haracter has not been found!", this.to);
                else
                {
                    var request = new MoveCharacterRequest { CharacterID = from.Character.ID, Position = to.Character.Position };
                    server.SendToAll(request, c => c.Character != null && !c.SwitchingMap);
                    return true;
                }
            }
            return false;
        }
    }
}