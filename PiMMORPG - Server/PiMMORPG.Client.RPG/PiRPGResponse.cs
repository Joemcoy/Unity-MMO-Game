using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Network;
using tFramework.Network.Bases;

namespace PiMMORPG.Client.RPG
{
    public abstract class PiRPGResponse : PiBaseResponse
    {
        public new PiRPGClient Client { get { return base.Client as PiRPGClient; } }
    }
}