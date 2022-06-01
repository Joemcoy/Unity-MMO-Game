using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Base.Factories;
using Game.Client;
using Game.Server;
using Game.Server.Writers;
using Network.Data.Interfaces;

namespace Game.Server.Responses
{
    public class TimeUpdatePacket : GCResponse
    {
        public override uint ID { get { return PacketID.TimeUpdate; } }

        public override bool Read(ISocketPacket Packet)
        {
            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Server = SingletonFactory.GetInstance<GameServer>();
            Socket.Send(new UpdateTimeWriter { Time = Server.ServerTime });
        }
    }
}