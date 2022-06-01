using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Network.Data;
using Network.Data.Interfaces;
using Game.Data;
using Game.Client;
using Game.Server.Writers;
using Base.Factories;

namespace Game.Server.Responses
{
    public class PingPacket : GCResponse
    {
        public override uint ID { get { return PacketID.Ping; } }
        long LastPing;

        public override bool Read(ISocketPacket Packet)
        {
            LastPing = Packet.ReadLong();
            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            Client.CurrentCharacter.LastPing = LastPing;

            PingWriter Packet = new PingWriter();
            Client.Socket.Send(Packet);
        }
    }
}
