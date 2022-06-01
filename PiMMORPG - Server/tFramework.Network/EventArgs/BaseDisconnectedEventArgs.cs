using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tFramework.Network.EventArgs
{
    using Enums;
    using Bases;
    using Interfaces;

    public class BaseDisconnectedEventArgs<TClient, TNetworkClient> : BaseClientEventArgs<TClient, TNetworkClient>
        where TClient : BaseClient<TClient, TNetworkClient>
        where TNetworkClient : INetworkClient<TNetworkClient>, new()
    {
        public DisconnectReason Reason { get; set; }

        public BaseDisconnectedEventArgs(TClient client, DisconnectReason reason) : base(client)
        {
            this.Reason = reason;
        }
    }
}