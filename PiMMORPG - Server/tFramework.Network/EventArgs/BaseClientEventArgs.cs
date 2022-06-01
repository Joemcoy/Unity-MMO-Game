using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tFramework.Network.EventArgs
{
    using Bases;
    using Interfaces;

    public class BaseClientEventArgs<TClient, TNetworkClient> : System.EventArgs
        where TClient : BaseClient<TClient, TNetworkClient>
        where TNetworkClient : INetworkClient<TNetworkClient>, new()
    {
        public TClient Client { get; set; }

        public BaseClientEventArgs(TClient client)
        {
            this.Client = client;
        }
    }
}