using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiMMORPG.Server.General.Drivers
{
    using Models;

    public class SpawnDriver : BaseDriver<Spawn>
    {
        protected override string TableName => "br_spawns";
    }
}