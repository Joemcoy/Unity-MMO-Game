using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tFramework.Network.EventArgs
{
    using Enums;
    using Bases;
    using Interfaces;

    public class BaseClientErrorEventArgs<TClient, TNetworkClient> : BaseClientEventArgs<TClient, TNetworkClient>
        where TClient : BaseClient<TClient, TNetworkClient>
        where TNetworkClient : INetworkClient<TNetworkClient>, new()
    {
        public Exception Error { get; set; }

        public BaseClientErrorEventArgs(TClient client, Exception error) : base(client)
        {
            Error = error;
        }
    }
}