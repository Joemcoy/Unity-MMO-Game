using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Data.Results
{
	public enum LoginResult : byte
	{
		Success = 0x1,
		InvalidUsername = 0x2,
		InvalidPassword = 0x3,
        NoGameGates = 0x04,
        InvalidVersion = 0x05,
        AccountBanned = 0x06,
        Error = 0x1,
        AlreadyLogged = 0x7,
    }
}
