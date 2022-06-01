using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Game.Client;

namespace Game.Server.EventArgs
{
    public class ClientDisconnectedEventArgs : System.EventArgs
    {
        public GameClient Client { get; private set; }

        public ClientDisconnectedEventArgs(GameClient Client)
        {
            this.Client = Client;
        }
    }
}
