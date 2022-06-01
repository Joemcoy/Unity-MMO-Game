using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiMMORPG.Server.BattleRoyale.Responses
{
    using General;
    using General.Drivers;
    public class SelectCharacterResponse : General.Responses.SelectCharacterResponse
    {
        protected override void LoadCharacter()
        {
            base.LoadCharacter();
            using (var ctx = new MapDriver())
            {
                Client.Character.Map = ctx.GetModel(ctx.CreateBuilder().Where(m => m.ID).Equal(ServerControl.Configuration.BattleRoyaleMap));
                Client.Character.Position = Client.Character.Map.Spawn;
            }

            using (var ctx = new CharacterItemDriver())
                packet.Items = Client.Character.Items = ctx.GetModels(ctx.CreateBuilder().Where(m => m.Character).Equal(Client.Character.ID).And(m => m.Equipped).Equal(true));
        }
    }
}