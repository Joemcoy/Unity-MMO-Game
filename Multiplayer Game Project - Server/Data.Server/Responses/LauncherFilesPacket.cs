using Data.Client;
using Game.Controller;
using Game.Data;
using Game.Data.Models;
using Network.Data;
using Network.Data.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Data.Server.Writers;

namespace Data.Server.Responses
{
    public class LauncherFilesPacket : DCResponse
    {
        public override uint ID { get { return PacketID.DataLauncherFileList; } }
        public override bool Read(ISocketPacket Packet)
        {
            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Packet = new LauncherFilesRequest();
            Packet.Files = LauncherFileManager.GetFiles();
            Client.Socket.Send(Packet);
        }
    }
}
