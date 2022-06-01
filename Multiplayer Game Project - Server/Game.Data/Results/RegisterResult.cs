using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Data.Results
{
    public enum RegisterResult : byte
    {
        Success = 0x5,
        InvalidRegister = 0x3,
        Error = 0x1,
        NonRegistered = 0x6,
    }
}
