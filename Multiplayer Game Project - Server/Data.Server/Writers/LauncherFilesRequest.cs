using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Game.Data.Models;
using Network.Data.Interfaces;

namespace Data.Server.Writers
{
    public class LauncherFilesRequest : IRequest
    {
        public uint ID { get { return PacketID.DataLauncherFileList; } }
        public LauncherFileModel[] Files { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            Packet.WriteInt(Files.Length);
            foreach (var File in Files)
                File.WritePacket(Packet);

            return true;
        }
    }
}
