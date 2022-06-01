using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Data.Results
{
    public struct CreateCharacterResult
    {
        public const byte Successful = 0x1;
        public const byte NameExists = 0x2;
        public const byte ReachedMaximum = 0x3;
        public const byte SlotInUse = 0x4;
        public const byte Error = 0x0;
    }
}
