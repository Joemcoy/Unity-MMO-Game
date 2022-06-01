using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Network.Data;
using Network.Data.Interfaces;
using Network.v1;

using Base.Factories;
using Data.Client;
using Game.Data;
using Game.Data.Models;

using Launcher.Server;
using System.Diagnostics;

namespace Data.Server.Packets
{
    class LauncherFilesPacket : DCResponse
    {
        LauncherFileModel[] Files;

        public override uint ID { get { return PacketID.DataLauncherFileList; } }
        public override bool Read(ISocketPacket Packet)
        {
            Files = new LauncherFileModel[Packet.ReadInt()];
            for(int i = 0; i < Files.Length; i++)
            {
                Files[i] = new LauncherFileModel();
                Files[i].ReadPacket(Packet);
            }

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Server = SingletonFactory.GetInstance<LauncherServer>();
            Server.Files = Files;

            LoggerFactory.GetLogger().LogSuccess("File list received, starting auth..");
            Process.Start("Auth.Service.exe");
        }
    }
}
