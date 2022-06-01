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
    public class RevivePlayerPacket : GCResponse
    {
        public override uint ID { get { return PacketID.Revive; } }
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
            Client.CurrentCharacter.Stats.Health = Client.CurrentCharacter.Stats.MaxHealth;
            Client.CurrentCharacter.Stats.Stamina = 100;
            Client.CurrentCharacter.Position = Client.CurrentMap.Spawn;

            GameServer Server = SingletonFactory.GetInstance<GameServer>();
            RevivePlayerWriter Packet = new RevivePlayerWriter();
            Packet.CID = Client.CurrentCharacter.ID;
            Packet.Position = Client.CurrentCharacter.Position;

            foreach (GameClient RClient in Server.Clients)
            {
                RClient.Socket.Send(Packet);
            }
        }
    }
}
