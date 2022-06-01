using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base.Factories;
using Data.Client;
using Game.Client;
using Game.Data;
using Game.Data.Models;
using Game.Server.Manager;
using Game.Server.Writers;
using Network.Data.Interfaces;

namespace Game.Server.DataResponses
{
    public class SendCharacterItemsPacket : DCResponse
    {
        CharacterItemModel[] Items;
        int OwnerID;

        public override uint ID { get { return PacketID.DataSendCharacterItems; } }

        public override bool Read(ISocketPacket Packet)
        {
            OwnerID = Packet.ReadInt();
            Items = new CharacterItemModel[Packet.ReadInt()];

            for (int i = 0; i < Items.Length; i++)
            {
                Items[i] = new CharacterItemModel();
                Items[i].ReadPacket(Packet);
            }
            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Client = this.Client.Dequeue<GameClient>();
            var Cache = SingletonFactory.GetInstance<ItemCacheManager>();

            LoggerFactory.GetLogger(this).LogWarning("Received items from database of character {0}...", OwnerID);

            Cache.AddCharacterItems(OwnerID, Items);
            Responses.SendCharacterItemsPacket.Send(Client, OwnerID, Items);
        }
    }
}
