using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Network.Data.Interfaces;

namespace Network.Data.EventArgs
{
    public class RequestEventArgs : BaseClientEventArgs
    {
        public IRequest Request { get; private set; }

        public RequestEventArgs(IClientSocket Client, IRequest Request)
            : base(Client)
        {
            this.Request = Request;
        }
    }
}
