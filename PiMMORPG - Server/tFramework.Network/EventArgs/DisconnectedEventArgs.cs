using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace tFramework.Network.EventArgs
{
    using Enums;
    using Interfaces;

    public class DisconnectedEventArgs<TNetworkClient> : ClientEventArgs<TNetworkClient>
        where TNetworkClient : INetworkClient<TNetworkClient>, new()
    {
        public DisconnectReason Reason { get; private set; }

        public DisconnectedEventArgs(TNetworkClient client, DisconnectReason reason) : base(client)
        {
            this.Reason = reason;
        }
    }
}