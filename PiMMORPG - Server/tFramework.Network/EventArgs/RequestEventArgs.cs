using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tFramework.Network.EventArgs
{
    using Bases;
    using Interfaces;
    public class RequestEventArgs<TNetworkClient> : ClientEventArgs<TNetworkClient>
        where TNetworkClient : INetworkClient<TNetworkClient>, new()
    {
        public IRequest<TNetworkClient> Request { get; private set; }
        public RequestEventArgs(TNetworkClient client, IRequest<TNetworkClient> request) : base(client)
        {
            Request = request;
        }
    }
}