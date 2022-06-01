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
    public class SendEquipsPacket : DCResponse
    {
        CharacterItemModel[] Items;

        public override uint ID { get { return PacketID.DataSendEquips; } }

        public override bool Read(ISocketPacket Packet)
        {
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

            CharacterListWriter Packet = new CharacterListWriter();
            Packet.Characters = Client.Characters;
            Packet.Items = Items;

            Client.Socket.Send(Packet);
        }
    }
}