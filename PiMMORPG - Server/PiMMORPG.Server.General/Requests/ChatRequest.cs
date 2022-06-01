using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Network.Interfaces;

namespace PiMMORPG.Server.General.Requests
{
    using Client;

    public class ChatRequest : PiBaseRequest
    {
        public override ushort ID => PacketID.Chat;
        public string Message { get; set; }

        public override bool Write(IDataPacket packet)
        {
            packet.WriteString(Message);
            return true;
        }
    }
}