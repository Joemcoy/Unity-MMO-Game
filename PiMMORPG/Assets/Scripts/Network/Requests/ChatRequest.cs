using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PiMMORPG;
using PiMMORPG.Client;
using tFramework.Network.Interfaces;

namespace Scripts.Network.Requests
{
    public class ChatRequest : PiBaseRequest
    {
        public override ushort ID { get { return PacketID.Chat; } }
        public string Message { get; set; }

        public override bool Write(IDataPacket Packet)
        {
            Packet.WriteString(Message);

            return true;
        }
    }
}
