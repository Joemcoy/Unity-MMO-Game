using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;

namespace tFramework.Network.EventArgs
{
    using Interfaces;
    public class ServerEventArgs<TNetworkServer, TNetworkClient> : System.EventArgs
        where TNetworkClient : INetworkClient<TNetworkClient>, new()
        where TNetworkServer : INetworkServer<TNetworkClient>, new()
    {
        public TNetworkServer Server { get; private set; }
        public IPEndPoint EndPoint { get; private set; }

        public ServerEventArgs(TNetworkServer server)
        {
            this.Server = server;
            EndPoint = server.EndPoint;
        }
    }
}
