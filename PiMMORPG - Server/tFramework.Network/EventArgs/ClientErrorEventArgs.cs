using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace tFramework.Network.EventArgs
{
    using Interfaces;
    public class ClientErrorEventArgs<TNetworkClient> : ClientEventArgs<TNetworkClient>
        where TNetworkClient : INetworkClient<TNetworkClient>, new()
    {
        public Exception Error { get; private set; }

        public ClientErrorEventArgs(TNetworkClient client, Exception error) : base(client)
        {
            this.Error = error;
        }
    }
}
