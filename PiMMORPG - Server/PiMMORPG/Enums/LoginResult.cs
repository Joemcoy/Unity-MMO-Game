using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiMMORPG.Enums
{
    public enum LoginResult : byte
    {
        Unknown = 0x0,
        Successful = 0x1,
        InvalidUsername = 0x2,
        InvalidPassword = 0x3,
        AlreadyLogged   = 0x4,
        Banned          = 0x5,
        InvalidVersion  = 0x6
    }
}
