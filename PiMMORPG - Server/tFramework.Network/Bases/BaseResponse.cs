using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace tFramework.Network.Bases
{
    using Interfaces;

    public abstract class BaseResponse<TClient, TNetworkClient> : IResponse<TNetworkClient>
        where TClient : BaseClient<TClient, TNetworkClient>
        where TNetworkClient : INetworkClient<TNetworkClient>, new()
    {
        public abstract ushort ID { get; }
        public TClient Client { get; internal set; }
        public TNetworkClient Socket { get; private set; }

        public abstract bool Read(IDataPacket packet);
        bool IResponse<TNetworkClient>.Read(TNetworkClient client, IDataPacket packet)
        {
            Socket = client;
            return Read(packet);
        }
        public abstract void Execute();

        IResponse<TNetworkClient> IResponse<TNetworkClient>.Clone()
        {
            return Clone() as IResponse<TNetworkClient>;
        }

        public BaseResponse<TClient, TNetworkClient> Clone()
        {
            var clone = Activator.CreateInstance(GetType()) as BaseResponse<TClient, TNetworkClient>;
            clone.Client = Client;
            clone.Socket = Socket;
            return clone;
        }
    }
}