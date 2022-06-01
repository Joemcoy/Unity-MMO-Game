using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Factories;
namespace PiMMORPG.Server.General.Commands
{
    using Requests;
    public class HelpCommand : BaseCommand
    {
        public override string Name => "help";
        public override string Description => "Show a list of available commands!";

        public override bool Parse(object caller, params string[] args)
        {
            if (!base.Parse(caller, args)) return false;
            else if (Client == null)
            {
                Logger.LogWarning("Client side only!");
                return false;

            }
            return true;
        }

        public override bool Execute()
        {
            var packet = new ChatRequest();
            var commands = CommandFactory.GetCommands<BaseCommand>();
            var available = commands.Where(c => c.AvailableFor(Client));

            packet.Message = string.Join(Environment.NewLine, available.Select(c => c.Name + " - " + c.Description).ToArray());
            Client.Socket.Send(packet);

            return true;
        }
    }
}
