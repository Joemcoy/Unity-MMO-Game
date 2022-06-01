using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using tFramework.Extensions;
namespace PiMMORPG.Server.BattleRoyale.Commands
{
    using Models;
    using Manager;
    using General.Drivers;
    using General.Requests;

    public class MoveToSpawn : BRCommand
    {
        public override string Name => "tospawn";
        public override string Description => "Move the sender to target spawn!";

        Position target;
        public override bool Parse(object caller, params string[] args)
        {
            if (!base.Parse(caller, args)) return false;
            else if (caller == null)
            {
                Logger.LogWarning("Only client command!");
                return false;
            }
            else if (Client.RoomID == Guid.Empty)
            {
                Logger.LogWarning("Only inside room command!");
                return false;
            }
            else if (args.Length == 1)
            {
                int id = int.Parse(args[0]);

                if (id > 0)
                {
                    using (var ctx = new SpawnDriver())
                        target = ctx.GetModel(ctx.CreateBuilder().Where(c => c.ID).Equal(id));
                }
                else
                {
                    using (var ctx = new MapSpawnDriver())
                        target = ctx.GetModel(ctx.CreateBuilder().Where(c => c.ID).Equal(General.ServerControl.Configuration.BattleRoyaleMap));
                }
                return target != null;
            }
            else return false;
        }

        public override bool Execute()
        {
            var room = RoomManager.GetRoomByID(Client.RoomID);
            if (room == null)
            {
                Logger.LogWarning("Client room not found!");
                return false;
            }
            else
            {
                Client.Character.Position = target;
                var packet = new MoveCharacterRequest { CharacterID = Client.Character.ID, Position = target };
                room.Clients.ForEach(c => c.Socket.Send(packet));

                Logger.LogSuccess("Moved character {0} to position {1}:{2},{3},{4}!", Client.Character.Name, target.ID, target.PositionX, target.PositionY, target.PositionZ);
                return true;
            }
        }
    }
}
