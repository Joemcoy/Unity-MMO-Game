using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiMMORPG.Server.General.Drivers
{
    using Models;

    public class MapSpawnDriver : BaseDriver<Position>
    {
        protected override string TableName => "map_spawns";
        public MapSpawnDriver()
        {
            
        }
    }
}