using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Network;

namespace PiMMORPG.Client.Interfaces
{
    using Models;

    public interface IGameClient
    {
        TCPAsyncClient Socket { get; }
        Account Account { get; set; }
        Character[] Characters { get; set; }
        Character Character { get; set; }

#if !UNITY_STANDALONE
        bool SwitchingMap { get; set; }
        bool CanSpawn(IGameClient other, bool allowSwitching);
#endif
    }
}