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
    public class RemoveItemPacket : GCResponse
    {
        Guid Serial;

        public override uint ID { get { return PacketID.RemoveItem; } }
        public override bool Read(ISocketPacket Packet)
        {
            if (Client != null && Client.CurrentCharacter != null)
            {
                Serial = Packet.ReadGuid();

                return true;
            }
            return false;
        }

        public override void Execute(IClientSocket Socket)
        {
            LoggerFactory.GetLogger(this).LogWarning("Removing item as {0}", Serial);

            var Cache = SingletonFactory.GetInstance<ItemCacheManager>();
            Cache.RemoveItem(Client.CurrentCharacter.ID, Serial);
        }
    }
}
