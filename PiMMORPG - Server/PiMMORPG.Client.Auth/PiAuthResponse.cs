using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Network;
using tFramework.Network.Bases;

namespace PiMMORPG.Client.Auth
{
    public abstract class PiAuthResponse : BaseResponse<PiAuthClient, TCPAsyncClient>
    {
#if UNITY_STANDALONE
        public virtual bool ThreadSafe { get; set; }
        public PiAuthResponse() { ThreadSafe = true; }
#endif
    }
}