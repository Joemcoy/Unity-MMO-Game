using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Network.Data.Interfaces;

namespace Network.Bases
{
    public abstract class ResponseBase<TClient> : IResponse
        where TClient : ClientBase<TClient>, new()
    {
        public abstract uint ID { get; }
        public TClient Client { get; internal set; }

        public abstract bool Read(ISocketPacket Packet);
        public abstract void Execute(IClientSocket Socket);
    }
}