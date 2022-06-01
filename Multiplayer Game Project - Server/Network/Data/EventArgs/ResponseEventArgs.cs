using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Network.Data.Interfaces;

namespace Network.Data.EventArgs
{
    public class ResponseEventArgs : BaseClientEventArgs
    {
        public IResponse Response { get; private set; }
        public ISocketPacket Packet { get; private set; }

        public ResponseEventArgs(IClientSocket Client, IResponse Response, ISocketPacket Packet) : base(Client)
        {
            this.Response = Response;
            this.Packet = Packet;
        }
    }
}
