using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Data.Client;
using Game.Data;
using Network.Data.Interfaces;
using Data.Server.Writers;

namespace Data.Server.Responses
{
    public class SendNPCsPacket : DCResponse
    {
        public override uint ID { get { return PacketID.DataSendNPCs; } }

        int Server;
        public override bool Read(ISocketPacket Packet)
        {
            Server = Packet.ReadInt();

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Packet = new SendNPCsRequest();
            Packet.Server = Server;

            Socket.Send(Packet);
        }
    }
}
