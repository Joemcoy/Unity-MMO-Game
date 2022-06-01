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
    public class SendMapByIDPacket : DCResponse
    {
        int MapID;
        bool SendItems;

        public override uint ID { get { return PacketID.DataSendMapByID; } }
        public override bool Read(ISocketPacket Packet)
        {
            MapID = Packet.ReadInt();
            SendItems = Packet.ReadBool();

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Packet = new MapRequest();
            Packet.Map = MapManager.GetMapByID(MapID);

            Client.Socket.Send(Packet);
        }
    }
}