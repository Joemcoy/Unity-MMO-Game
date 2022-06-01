using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;

using Network.Data.Interfaces;

namespace Network.Data.EventArgs
{
    public abstract class BaseClientEventArgs : System.EventArgs
    {
        public IClientSocket Client { get; private set; }
        public IServerSocket Server { get; private set; }
        public IPEndPoint EndPoint { get; private set; }

        public BaseClientEventArgs(IClientSocket Client)
        {
            this.Client = Client;
            Server = Client.Server;
            EndPoint = Client.EndPoint;
        }
    }
}
