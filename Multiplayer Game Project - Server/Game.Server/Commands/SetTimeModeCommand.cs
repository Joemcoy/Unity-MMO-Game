using Base.Factories;
using Game.Client;
using Gate.Client.Responses.Writers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Server.Manager;

namespace Game.Server.Commands
{
    public class SetTimeModeCommand : GCommand
    {
        public override string Name
        {
            get
            {
                return "timemode";
            }
        }

        public override bool Execute(params string[] Arguments)
        {
            GameClient Client = GetParameter<GameClient>("Client");
            if ((Client == null || Client.Account.Access == Data.Enums.AccessLevel.Administrator) && Arguments.Length > 0)
            {
                var Server = SingletonFactory.GetInstance<GameServer>();
                bool Update = Convert.ToInt32(Arguments[0]) == 1;

                WorldManager.UpdateTime = Update;
                return true;
            }
            else return false;
        }
    }
}
