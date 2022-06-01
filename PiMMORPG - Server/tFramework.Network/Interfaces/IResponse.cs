using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace tFramework.Network.Interfaces
{
    public interface IResponse<TNetworkClient>
        where TNetworkClient : INetworkClient<TNetworkClient>, new()
    {
        ushort ID { get; }

        bool Read(TNetworkClient client, IDataPacket packet);
        void Execute();

        IResponse<TNetworkClient> Clone();
    }
}
