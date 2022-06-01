using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gate.Client;
using Network.Data;
using Game.Data;
using Base.Factories;
using Game.Data.Enums;
using Network.Data.Interfaces;
using Game.Data.Information;

namespace Gate.Server.Responses
{
    public class GateTypePacket : GTResponse
    {
        public override uint ID { get { return PacketID.GateType; } }

        public override bool Read(ISocketPacket Packet)
        {
            Client.Type = Packet.ReadEnum<GateType>();

            GateInfo Info = new GateInfo();
            Info.ReadPacket(Packet);

            Client.Info = Info;

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            LoggerFactory.GetLogger(this).LogInfo($"Gate <{Client.Info.Address}> has changed to {Client.Type} type and port {Client.Port}!");
        }
    }
}