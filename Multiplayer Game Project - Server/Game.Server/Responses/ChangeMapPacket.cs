using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Game.Client;
using Game.Server;
using Game.Server.Manager;

using Network.Data.Interfaces;
using Game.Server.Writers;
using Base.Factories;
using Game.Data.Models;

namespace Game.Server.Responses
{
    public class ChangeMapPacket : GCResponse
    {
        public override uint ID { get { return PacketID.ChangeMap; } }

        int MapID;
        public override bool Read(ISocketPacket Packet)
        {
            MapID = Packet.ReadInt();
            LoggerFactory.GetLogger(this).LogWarning("Player {0} travels to map {1}", Client.CurrentCharacter.Name, MapID);

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Map = WorldManager.GetMapByID(MapID);
            if(Map != null)
            {
                WorldManager.TeleportPlayer(Client, Map);
            }
        }
    }
}