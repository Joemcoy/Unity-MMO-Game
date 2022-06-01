using Base.Factories;
using Game.Client;
using Gate.Client.Responses.Writers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Server.Writers;

namespace Game.Server.Commands
{
    public class SetTimeCommand : GCommand
    {
        public override string Name
        {
            get
            {
                return "timeset";
            }
        }

        public override bool Execute(params string[] Arguments)
        {
            GameClient Client = GetParameter<GameClient>("Client");
            if ((Client == null || Client.Account.Access == Data.Enums.AccessLevel.Administrator) && Arguments.Length > 0)
            {
                var Server = SingletonFactory.GetInstance<GameServer>();
                int Hour = Convert.ToInt32(Arguments[0]);
                int Minutes = Arguments.Length > 1 ? Convert.ToInt32(Arguments[1]) : Server.ServerTime.Minute;
                int Seconds = Arguments.Length > 2 ? Convert.ToInt32(Arguments[2]) : Server.ServerTime.Second;

                Server.ServerTime = Server.ServerTime.Date + new TimeSpan(Hour, Minutes, Seconds);

                var Packet = new UpdateTimeWriter();
                Packet.Time = Server.ServerTime;

                foreach (var Remote in Server.Clients.Where(C => C.CurrentCharacter != null))
                    Remote.Socket.Send(Packet);
                return true;
            }
            else return false;
        }
    }
}
