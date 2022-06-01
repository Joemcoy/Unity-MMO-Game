using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Network.Data.Interfaces;

namespace Data.Client.Requests
{
    public class LauncherFilesRequest : IRequest
    {
        public uint ID { get { return PacketID.DataLauncherFileList; } }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            return true;
        }
    }
}
