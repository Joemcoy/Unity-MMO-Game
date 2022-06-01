using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Data.Client;
using Game.Client;

using Network.Data;
using Network.Data.Interfaces;
using Game.Data;
using Game.Data.Models;
using Game.Server.Writers;
using Base.Factories;

namespace Game.Server.DataResponses
{
    public class SendCharactersPacket : DCResponse
    {
        CharacterModel[] Characters;

        public override uint ID { get { return PacketID.DataSendCharacters; } }
        public override bool Read(ISocketPacket Packet)
        {
            if (Packet.ReadBool())
            {
                Characters = new CharacterModel[Packet.ReadInt()];
                for (int i = 0; i < Characters.Length; i++)
                {
                    Characters[i] = new CharacterModel();
                    Characters[i].ReadPacket(Packet);
                }
            }
            else
                Characters = new CharacterModel[0];
            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            GameClient Client = this.Client.Dequeue<GameClient>();
            Client.Characters = Characters;

            if (Characters.Length > 0)
                this.Client.SendEquips(Client, Client.Account.ID);
            else
            {
                CharacterListWriter Packet = new CharacterListWriter();
                Packet.Characters = new CharacterModel[0];

                Client.Socket.Send(Packet);
            }
        }
    }
}