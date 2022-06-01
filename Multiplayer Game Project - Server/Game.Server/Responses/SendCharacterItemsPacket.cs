using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Game.Data.Models;
using Game.Data.Results;
using Game.Client;

using Server.Configuration;
using Network.Data.Interfaces;
using Game.Server.Writers;
using Game.Server.Manager;
using Base.Factories;
using Data.Client;

namespace Game.Server.Responses
{
    public class SendCharacterItemsPacket : GCResponse
    {
        int OwnerID;

        public override uint ID { get { return PacketID.SendCharacterItems; } }
        public override bool Read(ISocketPacket Packet)
        {
            OwnerID = Packet.ReadInt();
            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            LoggerFactory.GetLogger(this).LogWarning("Client {0} requested itens of character {1}!", Client.CurrentCharacter.ID, OwnerID);

            var Manager = SingletonFactory.GetInstance<ItemCacheManager>();
            var Items = Manager.GetCharacterItems(OwnerID);

            if (Items.Length == 0)
            {
                var Data = SingletonFactory.GetInstance<DataClient>();
                Data.SendCharacterItemsRequest(Client, OwnerID);
            }
            else
                Send(Client, OwnerID, Items);
        }

        public static void Send(GameClient Client, int OwnerID, CharacterItemModel[] Items)
        {
            var ItemsPacket = new SendCharacterItemsWriter();
            ItemsPacket.Items = Items;
            ItemsPacket.Owner = OwnerID;
            Client.Socket.Send(ItemsPacket);
        }
    }
}