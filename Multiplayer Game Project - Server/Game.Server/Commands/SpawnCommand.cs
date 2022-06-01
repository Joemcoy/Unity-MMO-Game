using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base.Factories;
using Game.Client;
using Game.Data.Enums;
using Game.Server.Manager;
using Game.Server.Writers;

namespace Game.Server.Commands
{
    public class SpawnCommand : GCommand
    {
        public override string Name { get { return "spawn"; } }

        public override bool Execute(params string[] Arguments)
        {
            GameClient Client = GetParameter<GameClient>("Client");
            if (Client != null && Client.Account.Access >= AccessLevel.Moderator)
            {
                var Server = SingletonFactory.GetInstance<GameServer>();
                var Map = WorldManager.GetMapByID(Client.CurrentMap.ID);

                Map.Spawn.CopyTo(Client.CurrentCharacter.Position);

                var Packet = new SetPlayerPositionWriter();
                Packet.CharacterID = Client.CurrentCharacter.ID;
                Packet.Position = Client.CurrentCharacter.Position;

                LoggerFactory.GetLogger(this).LogWarning("Sending player to position {0}", Packet.Position);

                foreach (var Remote in Server.Clients.Where(C => C.CurrentCharacter != null && C.CurrentMap.ID == Client.CurrentMap.ID))
                    Remote.Socket.Send(Packet);

                return true;
            }
            return false;
        }
    }
}