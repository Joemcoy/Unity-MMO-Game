using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

using Network.Data.Interfaces;

namespace Network.Data.EventArgs
{
    public abstract class BaseServerEventArgs : System.EventArgs
    {
        public IServerSocket Server { get; private set; }
        public IPEndPoint EndPoint { get; private set; }

        public BaseServerEventArgs(IServerSocket Server)
        {
            this.Server = Server;
            this.EndPoint = Server.EndPoint;
        }
    }
}
