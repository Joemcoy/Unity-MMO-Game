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
    public class SetEquipStatePacket : GCResponse
    {
        Guid Serial;
        bool State;

        public override uint ID { get { return PacketID.SetEquipState; } }
        public override bool Read(ISocketPacket Packet)
        {
            if (Client != null && Client.CurrentCharacter != null)
            {
                Serial = Packet.ReadGuid();
                State = Packet.ReadBool();

                return true;
            }
            return false;
        }

        public override void Execute(IClientSocket Socket)
        {
            LoggerFactory.GetLogger(this).LogWarning("{2} item {0} to character {1}..", Serial, Client.CurrentCharacter.ID, State ? "Equipping" : "UnEquipping");

            var Cache = SingletonFactory.GetInstance<ItemCacheManager>();
            Cache.SetItemState(Client.CurrentCharacter.ID, Serial, State);
        }
    }
}
