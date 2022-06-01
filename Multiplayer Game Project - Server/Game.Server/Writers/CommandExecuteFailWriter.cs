using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Base.Data;
using Network.Data;
using Network.Data.Interfaces;
using Game.Data;

namespace Game.Server.Writers
{
    public class CommandExecuteFailWriter : IRequest
    {
        public uint ID { get { return PacketID.CommandExecuteFail; } }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            return true;
        }
    }
}
