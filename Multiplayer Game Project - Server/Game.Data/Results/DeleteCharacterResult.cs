using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Data.Results
{
    public struct DeleteCharacterResult
    {
        public const byte Success = 0x01;
        public const byte NotFound = 0x02;
        public const byte Error = 0x03;
    }
}
