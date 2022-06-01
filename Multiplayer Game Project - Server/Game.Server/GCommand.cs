using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Base.Data.Abstracts;
using Game.Client;

namespace Game.Server
{
    public abstract class GCommand : ACommand
    {
        public GameClient Client { get; set; }
        public GameServer Server { get; set; }
    }
}
