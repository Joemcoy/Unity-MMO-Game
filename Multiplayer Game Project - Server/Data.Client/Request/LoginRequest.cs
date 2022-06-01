using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Network.Data.Interfaces;

namespace Data.Client.Requests
{
    public class LoginRequest : IRequest
    {
        public uint ID { get { return PacketID.DataSendLogin; } }
        public string Username { get; set; }
        public string Password { get; set; }
        public uint Server { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            Packet.WriteString(Username);
            Packet.WriteString(Password);
            Packet.WriteUInt(Server);
            return true;
        }
    }
}
