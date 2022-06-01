using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Network.Data.Interfaces;
using Network.Data;
using Game.Data;
using Game.Data.Enums;

namespace Gate.Client.Responses.Writers
{
    public class GlobalMessageWriter : IRequest
    {
        public uint ID { get { return PacketID.SendGlobalMessage; } }

        public string Sender { get; set; }
        public string Message { get; set; }

        public bool Write(IClientSocket Client, ISocketPacket Packet)
        {
            Packet.WriteString(Sender ?? "SERVER");
            Packet.WriteString(Message);
            return true;
        }
    }
}
