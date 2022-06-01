using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Game.Data;
using Game.Data.Results;
using Network.Data.Interfaces;
using Server.Configuration;
using Base.Factories;

namespace Launcher.Server.Writers
{
    public class LauncherFileListWriter : IRequest
    {
        public uint ID { get { return PacketID.LauncherFileList; } }
        public LauncherResult Result { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            Packet.WriteEnum(Result);
            if (Result == LauncherResult.Successfull)
            {
                Packet.WriteString("");

                var Server = SingletonFactory.GetInstance<LauncherServer>();

                Packet.WriteInt(Server.Files.Length);
                foreach (var File in Server.Files)
                    File.WritePacket(Packet);
            }

            return true;
        }
    }
}
