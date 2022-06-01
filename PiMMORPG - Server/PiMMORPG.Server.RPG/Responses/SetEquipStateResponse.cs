using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiMMORPG.Server.RPG.Responses
{
    using General.Drivers;

    public class SetEquipStateResponse : General.Responses.SetEquipStateResponse
    {
        public override void Execute()
        {
            base.Execute();
            using (var ctx = new CharacterItemDriver())
                ctx.UpdateModel(item);
        }
    }
}