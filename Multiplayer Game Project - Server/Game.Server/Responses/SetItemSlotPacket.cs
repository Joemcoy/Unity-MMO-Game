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


namespace Game.Server.Responses
{
    public class SetItemSlotPacket : GCResponse
    {
        Guid Serial;
        uint Slot;

        public override uint ID { get { return PacketID.SetItemSlot; } }
        public override bool Read(ISocketPacket Packet)
        {
            if (Client != null && Client.CurrentCharacter != null)
            {
                Serial = Packet.ReadGuid();
                Slot = Packet.ReadUInt();

                return true;
            }
            return false;
        }

        public override void Execute(IClientSocket Socket)
        {
            LoggerFactory.GetLogger(this).LogWarning("Set slot of item {0} to {1} of character {2}..", Serial, Slot, Client.CurrentCharacter.ID);

            var Cache = SingletonFactory.GetInstance<ItemCacheManager>();
            Cache.SetItemSlot(Client.CurrentCharacter.ID, Serial, Slot);
        }
    }
}
