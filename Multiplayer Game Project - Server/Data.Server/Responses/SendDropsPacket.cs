using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base.Factories;
using Data.Client;
using Data.Server.Writers;
using Game.Data;
using Game.Data.Models;
using Network.Data.Interfaces;

namespace Data.Server.Responses
{
    public class SendDropsPacket : DCResponse
    {
        public override uint ID { get { return PacketID.DataSendDrops; } }
        
        public override bool Read(ISocketPacket Packet)
        {
            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Base = ControllerFactory.GetBaseController("drops");

            var Packet = new SendDropsRequest();
            var Drops = Base.GetModels<DropModel>();
            
            foreach (var Drop in Drops)
            {
                Packet.Drop = Drop;
                Packet.End = false;
                Socket.Send(Packet);
            }

            Packet.Drop = null;
            Packet.End = true;

            Socket.Send(Packet);
        }
    }
}
