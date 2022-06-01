using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiMMORPG.Server.General.Drivers
{
    using Models;

    public class CharacterPositionDriver : BaseDriver<Position>
    {
        protected override string TableName => "character_position";
        public CharacterPositionDriver()
        {
            
        }
    }
}