using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiMMORPG.Server.RPG.Responses
{
    using General.Drivers;

    public class RequestMapDataResponse : General.Responses.RequestMapDataResponse
    {
        public override void Execute()
        {
            packet.Characters = server.Clients.Where(c => Client.CanSpawn(c, true)).Select(c => c.Character).ToArray();
            using (var ctx = new DropDriver())
                packet.Drops = ctx.GetModels(ctx.CreateBuilder().Where(m => m.Map).Equal(Client.Character.Map));
            using (var ctx = new TreeDriver())
                packet.Trees = ctx.GetModels(ctx.CreateBuilder().Where(m => m.Map).Equal(Client.Character.Map));
            base.Execute();
        }
    }
}