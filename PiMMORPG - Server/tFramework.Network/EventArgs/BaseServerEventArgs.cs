using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tFramework.Network.EventArgs
{
    using Bases;
    using Interfaces;

    public class BaseServerEventArgs<TServer, TNetworkServer, TClient, TNetworkClient> : System.EventArgs
        where TServer : BaseServer<TServer, TNetworkServer, TClient, TNetworkClient>, new()
        where TNetworkServer : INetworkServer<TNetworkServer, TNetworkClient>, new()
        where TClient : BaseClient<TClient, TNetworkClient>
        where TNetworkClient : INetworkClient<TNetworkClient>, new()
    {
        public TServer Server { get; set; }

        public BaseServerEventArgs(TServer server)
        {
            this.Server = server;
        }
    }
}