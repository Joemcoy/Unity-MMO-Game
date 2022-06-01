using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gate.Client;
using Game.Data;
using Game.Data.Models;
using Network.Data.Interfaces;
using Gate.Client.Responses.Writers;

namespace Gate.Server.Responses
{
    public class GlobalMessagePacket : GTResponse
    {
        public override uint ID { get { return PacketID.SendGlobalMessage; } }

        string Sender, Message;
        public override bool Read(ISocketPacket Packet)
        {
            Sender = Packet.ReadString();
            Message = Packet.ReadString();

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Packet = new GlobalMessageWriter();
            Packet.Message = Message;
            Packet.Sender = Sender;
            
            foreach(var Gate in Client.Server.Clients.Where(G => G.GatePort != Client.GatePort))
            {
                Gate.Socket.Send(Packet);
            }
        }
    }
}
