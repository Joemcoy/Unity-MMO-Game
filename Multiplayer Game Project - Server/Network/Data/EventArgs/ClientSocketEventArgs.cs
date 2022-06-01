using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

using Network.Data.Interfaces;

namespace Network.Data.EventArgs
{
    public class ClientSocketEventArgs : BaseClientEventArgs
    {
        public ClientSocketEventArgs(IClientSocket Client)
            : base(Client)
        {

        }
    }
}