using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Network.Data.Interfaces;
using Network.Data;
using Game.Data;
using Game.Data.Enums;
using Game.Data.Information;

namespace Gate.Client.Responses.Writers
{
    public class SendGateTypeWriter : IRequest
    {
        public uint ID { get { return PacketID.GateType; } }

        public GateType Type { get; set; }
        public GateInfo Gate { get; set; }

        public bool Write(IClientSocket Client, ISocketPacket Packet)
        {
            Packet.WriteEnum(Type);
            Gate.WritePacket(Packet);
            
            return true;
        }
    }
}
