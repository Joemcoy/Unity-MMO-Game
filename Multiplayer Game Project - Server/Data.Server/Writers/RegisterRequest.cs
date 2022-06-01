using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Game.Data.Results;
using Network.Data.Interfaces;

namespace Data.Server.Writers
{
    public class RegisterRequest : IRequest
    {
        public uint ID { get { return PacketID.DataSendRegister; } }
        public RegisterResult Result { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            Packet.WriteEnum(Result);

            return true;
        }
    }
}
