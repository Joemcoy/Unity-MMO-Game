using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiMMORPG.Client.BattleRoyale
{
    public abstract class PiBRResponse : PiBaseResponse
    {
        public new PiBRClient Client { get { return base.Client as PiBRClient; } }
    }
}