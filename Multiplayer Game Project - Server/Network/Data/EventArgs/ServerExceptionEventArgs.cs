using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Network.Data.Interfaces;

namespace Network.Data.EventArgs
{
    public class ServerExceptionEventArgs : BaseServerEventArgs
    {
        public Exception Error { get; private set; }
        
        public ServerExceptionEventArgs(IServerSocket Server, Exception Error) : base(Server)
        {
            this.Error = Error;
        }
    }
}
