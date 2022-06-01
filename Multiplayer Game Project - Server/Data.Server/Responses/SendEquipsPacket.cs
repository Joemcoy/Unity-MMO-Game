using Data.Client;
using Game.Controller;
using Game.Data;
using Game.Data.Models;
using Network.Data;
using Network.Data.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Data.Server.Writers;

namespace Data.Server.Responses
{
    class SendEquipsPacket : DCResponse
    {
        int AccountID;

        public override uint ID { get { return PacketID.DataSendEquips; } }
        public override bool Read(ISocketPacket Packet)
        {
            AccountID = Packet.ReadInt();

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Characters = CharacterManager.GetCharactersByAID(AccountID);

            var Packet = new SendEquipsRequest();
            Packet.Items = new CharacterItemModel[0];

            foreach (var Character in Characters)
                Packet.Items = Packet.Items.Concat(CharacterItemManager.GetEquipsByOwner(Character.ID)).ToArray();

            Client.Socket.Send(Packet);
        }
    }
}
