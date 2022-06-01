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
using Server.Configuration;

namespace Game.Server.Responses
{
    public class AttackPlayerPacket : GCResponse
    {
        string Name;
        uint Damage;
        bool Front;

        public override uint ID { get { return PacketID.AttackPlayer; } }
        public override bool Read(ISocketPacket Packet)
        {
            if (Client != null && Client.CurrentCharacter != null)
            {
                Name = Packet.ReadString();
                Damage = Packet.ReadUInt();
                Front = Packet.ReadBool();

                return true;
            }
            return false;
        }

        public override void Execute(IClientSocket Socket)
        {
            GameServer Server = SingletonFactory.GetInstance<GameServer>();
            GameClient Target = Server.Clients.FirstOrDefault(C => C.CurrentCharacter != null && C.CurrentCharacter.Name == Name);
            
            if (Target != null)
            {
                Target.CurrentCharacter.Stats.Health -= Damage;
                if (Target.CurrentCharacter.Stats.Health <= 0)
                {
                    Target.CurrentCharacter.Stats.Health = 0;

                    Client.CurrentCharacter.Stats.Experience += GameConfiguration.BaseExperience * Target.CurrentCharacter.Stats.Level;
                    if (Client.CurrentCharacter.Stats.Experience > Client.CurrentCharacter.Stats.Level * GameConfiguration.BaseExperience)
                        Client.CurrentCharacter.Stats.Level++;
                }

                AttackPlayerWriter AttackPacket = new AttackPlayerWriter();
                AttackPacket.NameA = Name;
                AttackPacket.NameB = Client.CurrentCharacter.Name;
                AttackPacket.Damage = Damage;
                AttackPacket.Front = true;
                
                KillPlayerWriter KillPacket = new KillPlayerWriter();
                KillPacket.Target = Name;
                KillPacket.Enemy = Client.CurrentCharacter.Name;
                KillPacket.Level = Client.CurrentCharacter.Stats.Level;
                KillPacket.Experience = Client.CurrentCharacter.Stats.Experience;

                foreach (GameClient RClient in Server.Clients)
                {
                    if (Target.CurrentCharacter.Stats.Health == 0)
                    {
                        RClient.Socket.Send(KillPacket);
                    }
                    else
                    {
                        RClient.Socket.Send(AttackPacket);
                    }
                }
            }
            else
                LoggerFactory.GetLogger(this).LogWarning("Failed to find client {0}", Name);
        }
    }
}