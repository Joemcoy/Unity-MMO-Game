using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Network.Data.Interfaces;

namespace Network.Data.EventArgs
{
    public class ClientConnectedEventArgs : BaseServerEventArgs
    {
        public IClientSocket Client { get; private set; }

        public ClientConnectedEventArgs(IServerSocket Server, IClientSocket Client) : base(Server)
        {
            this.Client = Client;
        }
    }
}
