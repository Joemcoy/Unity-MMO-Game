using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Network.Data;
using Network.Data.Interfaces;

namespace Launcher.Client
{
    public abstract class LCReader : IResponse
    {
        public LauncherClient Client { get; set; }
        public abstract uint ID { get; }

        public abstract bool Read(ISocketPacket Packet);
        public abstract void Execute(IClientSocket Socket);
    }
}
