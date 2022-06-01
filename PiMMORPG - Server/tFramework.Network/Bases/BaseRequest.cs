using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tFramework.Network.Bases
{
    using Interfaces;

    public abstract class BaseRequest<TClient, TNetworkClient> : IRequest<TNetworkClient>
        where TClient : BaseClient<TClient, TNetworkClient>
        where TNetworkClient : INetworkClient<TNetworkClient>, new()
    {
        public abstract ushort ID { get; }
        public TClient Client { get; internal set; }
        public TNetworkClient Socket { get; private set; }

        public abstract bool Write(IDataPacket packet);
        bool IRequest<TNetworkClient>.Write(TNetworkClient client, IDataPacket packet)
        {
            Socket = client;
            return Write(packet);
        }

        public BaseRequest<TClient, TNetworkClient> Clone()
        {
            var clone = Activator.CreateInstance(GetType()) as BaseRequest<TClient, TNetworkClient>;
            clone.Client = Client;
            clone.Socket = Socket;
            return clone;
        }
    }
}
