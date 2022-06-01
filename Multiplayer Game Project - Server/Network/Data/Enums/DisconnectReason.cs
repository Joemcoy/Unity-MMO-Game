using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Network.Data.Enums
{
    public enum DisconnectReason : byte
    {
        Normal = 1,
        EndOfStream = 2,
        MaximumReached = 3,
        Error = 4,
        Unknown = 0
    }
}
