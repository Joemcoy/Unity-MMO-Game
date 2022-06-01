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
    public class SetItemAmountPacket : GCResponse
    {
        Guid Serial;
        uint Amount;

        public override uint ID { get { return PacketID.SetItemAmount; } }
        public override bool Read(ISocketPacket Packet)
        {
            if (Client != null && Client.CurrentCharacter != null)
            {
                Serial = Packet.ReadGuid();
                Amount = Packet.ReadUInt();

                return true;
            }
            return false;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Cache = SingletonFactory.GetInstance<ItemCacheManager>();
            Cache.SetItemAmount(Client.CurrentCharacter.ID, Serial, Amount);
        }
    }
}
