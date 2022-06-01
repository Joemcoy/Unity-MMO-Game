using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace tFramework.Network.Interfaces
{
    public interface IRequest<TNetworkClient>
        where TNetworkClient : INetworkClient<TNetworkClient>, new()
    {
        ushort ID { get; }
        bool Write(TNetworkClient client, IDataPacket packet);
    }
}
