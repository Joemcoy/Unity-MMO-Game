using Base.Factories;
using Game.Client;
using Gate.Client.Responses.Writers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Game.Server.Commands
{
    public class HelpCommand : GCommand
    {
        public override string Name
        {
            get
            {
                return "help";
            }
        }

        public override bool Execute(params string[] Arguments)
        {
            var Client = GetParameter<GameClient>("Client");
            var Factory = SingletonFactory.GetInstance<CommandFactory<GCommand>>();

            foreach(GCommand Command in Factory.GetCommands())
            {
                if (Client == null)
                    LoggerFactory.GetLogger(this).LogInfo("-- {0}", Command.Name);
                else
                {
                    GlobalMessageWriter Packet = new GlobalMessageWriter();
					Packet.Message = string.Format("[{0}]: {1}", Client.CurrentCharacter.Name, string.Join (" ", Arguments));

                    Client.Socket.Send(Packet);
                }
            }
            return true;
        }
    }
}
