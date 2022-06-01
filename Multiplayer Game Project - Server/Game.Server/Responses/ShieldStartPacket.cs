using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Network.Data;
using Network.Data.Interfaces;
using Game.Data;
using Game.Client;
using Data.Client;
using Base.Factories;

using Game.Data.Information;
using Game.Data.Enums;
using Game.Server.Manager;
using Game.Data.Models;
using Game.Server.Writers;
using Gate.Client.Responses.Writers;

namespace Game.Server.Responses
{
    public class ShieldStartPacket : GCResponse
    {
        public override uint ID { get { return PacketID.ShieldStart; } }
        public override bool Read(ISocketPacket Packet)
        {
            if (Client != null && Client.CurrentCharacter != null)
            {
                return true;
            }
            return false;
        }

        public override void Execute(IClientSocket Socket)
        {
            GameServer Server = SingletonFactory.GetInstance<GameServer>();
            ShieldStartWriter Packet = new ShieldStartWriter();
            Packet.CID = Client.CurrentCharacter.ID;

            foreach (GameClient RClient in Server.Clients)
            {
                RClient.Socket.Send(Packet);
            }
        }
    }
}
