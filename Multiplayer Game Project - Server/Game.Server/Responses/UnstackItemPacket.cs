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
using Game.Server.Manager;

namespace Game.Server.Responses
{
    public class UnstackItemPacket : GCResponse
    {
        uint Start, End, Amount;
        Guid FromSerial, ToSerial;

        public override uint ID { get { return PacketID.UnstackItem; } }
        public override bool Read(ISocketPacket Packet)
        {
            if (Client != null && Client.CurrentCharacter != null)
            {
                Start = Packet.ReadUInt();
                End = Packet.ReadUInt();
                Amount = Packet.ReadUInt();
                FromSerial = Packet.ReadGuid();
                ToSerial = Packet.ReadGuid();

                return true;
            }
            return false;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Cache = SingletonFactory.GetInstance<ItemCacheManager>();
            Cache.UnstackItem(Client.CurrentCharacter.ID, Start, End, Amount, FromSerial, ToSerial);
        }
    }
}
