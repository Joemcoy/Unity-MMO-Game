using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Network.Data.Interfaces;
using Network.Data.Enums;

namespace Network.Data.EventArgs
{
    public class ClientDisconnectedEventArgs : BaseClientEventArgs
    {
        public DisconnectReason Reason { get; private set; }

        public ClientDisconnectedEventArgs(IClientSocket Client, DisconnectReason Reason) 
            : base(Client)
        {
            this.Reason = Reason;
        }
    }
}
