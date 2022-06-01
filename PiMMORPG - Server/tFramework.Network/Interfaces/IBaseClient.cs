using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tFramework.Network.Interfaces
{
    public interface IBaseClient<TNetworkClient>
        where TNetworkClient : INetworkClient<TNetworkClient>, new()
    {
        TNetworkClient Socket { get; }
    }
}