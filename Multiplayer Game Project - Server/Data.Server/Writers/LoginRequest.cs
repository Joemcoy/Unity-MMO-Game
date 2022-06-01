using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Game.Data.Models;
using Game.Data.Results;
using Network.Data.Interfaces;

namespace Data.Server.Writers
{
    public class LoginRequest : IRequest
    {
        public uint ID { get { return PacketID.DataSendLogin; } }
        public LoginResult Result {get;set;}
        public AccountModel Account { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            Packet.WriteEnum(Result);
            if (Result == LoginResult.Success)
                Account.WritePacket(Packet);

            return true;
        }
    }
}
