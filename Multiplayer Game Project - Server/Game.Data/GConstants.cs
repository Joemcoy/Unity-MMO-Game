using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Data.Information;

namespace Game.Data
{
    public static class GConstants
    {
        public static readonly VersionInfo Version = new VersionInfo(0, 0, 3, 3, "Alpha");

#if !UNITY_5
        public const bool DataDebug = false;
#endif

#if !DEBUG
        public const bool Debug = true;
#else
        public const bool Debug = false;
#endif
    }
}
