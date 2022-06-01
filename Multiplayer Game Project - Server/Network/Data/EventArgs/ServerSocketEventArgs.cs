using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Network.Data.Interfaces;

namespace Network.Data.EventArgs
{
    public class ServerSocketEventArgs : BaseServerEventArgs
    {
        public ServerSocketEventArgs(IServerSocket Socket) : base(Socket)
        {

        }
    }
}
