using Base.Data.Attributes;
using Base.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network.ArgumentReceivers
{
    public class SocketArguments : IArgumentReceiver
    {
        [Argument("DebugSocket")]
        public static bool DebugSocket { get; set; }
    }
}
