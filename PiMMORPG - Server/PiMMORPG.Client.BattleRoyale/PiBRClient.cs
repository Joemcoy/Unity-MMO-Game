using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiMMORPG.Client.BattleRoyale
{
    public class PiBRClient : PiBaseClient
    {
#if UNITY_STANDALONE
        public PiBRClient()
        {
            RegisterResponses();
            RegisterResponses<PiBRResponse>();
        }
#else
        public Guid RoomID { get; set; }
#endif
    }
}