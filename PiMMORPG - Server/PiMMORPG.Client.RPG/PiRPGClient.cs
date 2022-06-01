using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiMMORPG.Client.RPG
{
    public class PiRPGClient : PiBaseClient
    {
#if UNITY_STANDALONE
        public PiRPGClient()
        {
            RegisterResponses();
            RegisterResponses<PiRPGResponse>();
        }
#endif
    }
}