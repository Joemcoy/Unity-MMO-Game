using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PiMMORPG;
using PiMMORPG.Client.Auth;
using tFramework.Network.Interfaces;

namespace Scripts.Network.Requests
{
    public class LoginRequest : PiAuthRequest
    {
        public override ushort ID { get { return PacketID.Login; } }
        public string Username { get; set; }
        public string Password { get; set; }

        public override bool Write(IDataPacket Packet)
        {
            Packet.WriteString(PiConstants.Version);
            Packet.WriteString(Username);
            Packet.WriteString(Password);

            return true;
        }
    }
}
