using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Launcher.Client;
using Game.Data;
using Network.Data.Interfaces;
using Launcher.Server.Writers;
using Game.Data.Results;

namespace Launcher.Server.LauncherPackets
{
    public class LauncherFileListPacket : LCReader
    {
        public override uint ID { get { return PacketID.LauncherFileList; } }


        short Major, Minor;
        public override bool Read(ISocketPacket Packet)
        {
            Major = Packet.ReadInt16();
            Minor = Packet.ReadInt16();

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Writer = new LauncherFileListWriter();
            if (Major != GConstants.MajorVersion)
                Writer.Result = LauncherResult.InvalidMajor;
            else if (Minor != GConstants.MinorVersion)
                Writer.Result = LauncherResult.InvalidMinor;
            else
                Writer.Result = LauncherResult.Successfull;
            Socket.Send(Writer);
        }
    }
}
