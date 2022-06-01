using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Factories;
namespace PiMMORPG.Server.RPG.Manager
{
    using Client.RPG;
    using Models;

    using General;
    using General.Drivers;
    using General.Requests;
    using General.Interfaces;

    public static class WorldManager
    {
        public static void DropItem(PiRPGClient client, Drop drop)
        {
            var server = ServerControl.GetServer(client.Socket.Server.EndPoint.Port);
            DropItem(server, drop);
        }

        public static void DropItem(IGameServer server, Drop drop)
        {
            using (var ctx = new CharacterItemDriver())
            {
                //ctx.RemoveModel(ctx.CreateBuilder().Where(m => m.Serial).Equal(drop.Serial));
                var query = ctx.CreateBuilder().Where(m => m.Serial).Equal(drop.Serial);
                var item = ctx.GetModel(query);
                ctx.RemoveModel(query);
            }

            using (var ctx = new DropDriver())
            {
                ctx.AddModel(drop);

                var packet = new SendDropRequest();
                packet.Drop = drop;

                foreach (var remote in server.Clients.Where(c => !c.SwitchingMap && c.Character != null && c.Character.Map.ID == drop.Map.ID))
                    remote.Socket.Send(packet);
            }
        }
    }
}