using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace tFramework.Network.EventArgs
{
    using Interfaces;

    public class PacketEventArgs<TNetworkClient> : ClientEventArgs<TNetworkClient>
        where TNetworkClient : INetworkClient<TNetworkClient>, new()
    {
        public IDataPacket Packet { get; private set; }

        public PacketEventArgs(TNetworkClient client, IDataPacket packet) : base(client)
        {
            this.Packet = packet;
        }
    }
}