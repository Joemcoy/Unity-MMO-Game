using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiMMORPG.Server.General.Drivers
{
    using Models;

    public class CharacterItemDriver : BaseDriver<CharacterItem>
    {
        protected override string TableName => "character_items";
        public CharacterItemDriver()
        {
            MapDriver<ItemDriver>(m => m.Info);
        }
    }
}