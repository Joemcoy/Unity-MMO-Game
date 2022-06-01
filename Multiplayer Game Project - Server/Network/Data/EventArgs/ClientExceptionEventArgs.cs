using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Network.Data.Interfaces;

namespace Network.Data.EventArgs
{
    public class ClientExceptionEventArgs : BaseClientEventArgs
    {
        public Exception Error { get; private set; }

        public ClientExceptionEventArgs(IClientSocket Client, Exception Error) : base(Client)
        {
            this.Error = Error;
        }
    }
}
