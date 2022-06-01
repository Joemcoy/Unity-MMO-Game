using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Factories;
namespace PiMMORPG.Server.General.Commands
{
    using Models;

    public class SystemCommand : BaseCommand
    {
        public override string Name => "system";
        public override string Description => "Sends a message to all players in the channel!";

        string message;
        public override bool Parse(object caller, params string[] args)
        {
            base.Parse(caller, args);
            if (Client == null)
                LoggerFactory.GetLogger(this).LogError("Only client command!");
            else
            {
                message = string.Join(" ", args);
                return true;
            }
            return false;
        }

        public override bool Execute()
        {
            foreach(var server in ServerControl.Servers)
                server.SendSystemMessage(message);

            return true;
        }
    }
}