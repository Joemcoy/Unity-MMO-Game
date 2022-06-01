using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Network.Data;
using Network.Data.Interfaces;

using Base.Factories;
using Data.Client;
using Game.Data;
using Game.Data.Models;

using System.Diagnostics;
using Game.Data.Information;
using Server.Configuration;
using Base;

namespace Auth.Server.Responses
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
            var Server = SingletonFactory.GetInstance<AuthServer>();
            Server.Files = Files;

            LoggerFactory.GetLogger(this).LogSuccess("File list received, starting gates..");
            foreach (GateInfo Gate in GatesConfiguration.Gates)
            {
                BaseHooks.StartProcess("Game.Service.exe", string.Format("{0} {1} {2} {3} {4}", Gate.Port, Gate.PVP ? 1 : 0, Gate.MaximumClients, Gate.Address, Gate.Name));
            }
        }
    }
}
