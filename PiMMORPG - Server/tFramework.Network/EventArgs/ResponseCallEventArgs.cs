using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tFramework.Network.EventArgs
{
    using Interfaces;

    public class ResponseCallEventArgs<TNetworkClient> : ClientEventArgs<TNetworkClient>
        where TNetworkClient : INetworkClient<TNetworkClient>, new()
    {
        public IResponse<TNetworkClient> Response { get; private set; }
        public Action Callback { get; private set; }
        public bool CancelCall { get; set; }

        public ResponseCallEventArgs(TNetworkClient client, IResponse<TNetworkClient> response, Action callback) : base(client)
        {
            this.Response = response;
            this.Callback = callback;
            CancelCall = false;
        }
    }
}
