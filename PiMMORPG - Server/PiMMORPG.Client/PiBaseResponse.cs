using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Network;
using tFramework.Network.Bases;

namespace PiMMORPG.Client
{
    public abstract class PiBaseResponse<TClient> : PiBaseResponse
        where TClient : BaseClient<PiBaseClient, TCPAsyncClient>
    {
        public new TClient Client { get { return base.Client as TClient; } }
    }

    public abstract class PiBaseResponse : BaseResponse<PiBaseClient, TCPAsyncClient>
    {
#if UNITY_STANDALONE
        public virtual bool ThreadSafe { get; set; }
        public PiBaseResponse() { ThreadSafe = true; }
#endif
    }
}