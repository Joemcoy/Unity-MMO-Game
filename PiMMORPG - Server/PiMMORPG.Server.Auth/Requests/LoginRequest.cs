using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Network.Interfaces;

namespace PiMMORPG.Server.Auth.Requests
{
    using Enums;
    using Models;
    using General;
    using Client.Auth;

    public class LoginRequest : PiAuthRequest
    {
        public override ushort ID => PacketID.Login;

        public LoginResult Result { get; set; }
        public Account User { get; set; }

        public override bool Write(IDataPacket packet)
        {
            packet.WriteEnum(Result);
            if (Result == LoginResult.Successful)
            {
                packet.WriteWrapper(User);
                packet.WriteWrappers(ServerControl.Servers.Select(c =>
                {
                    var ch = c.Channel;
                    ch.Connections = c.Clients.Length;
                    return ch;
                }).ToArray());
                packet.WriteString(ServerControl.Configuration.ChecksumMD5);
            }
            return true;
        }
    }
}