using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;


namespace tFramework.Network.EventArgs
{
    using Interfaces;

    public class ClientEventArgs<TNetworkClient> : System.EventArgs
        where TNetworkClient : INetworkClient<TNetworkClient>, new()
    {
        public TNetworkClient Client { get; private set; }
        public IPEndPoint EndPoint { get { return Client.EndPoint; } }

        public ClientEventArgs(TNetworkClient client)
        {
            this.Client = client;
        }
    }
}
