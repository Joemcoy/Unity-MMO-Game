using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Network.Data;
using Network.Data.Interfaces;
using Game.Data;
using Game.Data.Models;
using Game.Controller;

using Data.Client;

using Data.Server.Writers;

namespace Data.Server.Responses
{
    public class SendMapsPacket : DCResponse
    {
        public override uint ID { get { return PacketID.DataSendMaps; } }
        public override bool Read(ISocketPacket Packet)
        {
            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Packet = new MapsRequest();
            Packet.Maps = MapManager.GetMaps();

            Client.Socket.Send(Packet);
        }
    }
}