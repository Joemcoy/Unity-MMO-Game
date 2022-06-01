using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Data.Client;
using Game.Controller;
using Game.Data.Models;
using Game.Data.Results;
using Server.Configuration;
using Network.Data.Interfaces;
using Network.Data;
using Base.Factories;
using Data.Server.Writers;

namespace Data.Server.Responses
{
    public class CreateCharacterPacket : DCResponse
    {
        CharacterModel Character;

        public override uint ID { get { return PacketID.DataCreateCharacter; } }
        public override bool Read(ISocketPacket Packet)
        {
            Character = new CharacterModel();
            Character.ReadPacket(Packet);

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Packet = new CreateCharacterRequest();

            CharacterModel OldCharacter = CharacterManager.GetCharacterByName(Character.Name);
            int Count = CharacterManager.CountCharacterByAID(Character.AID);

            if (OldCharacter != null)
            {
                Packet.Result = CreateCharacterResult.NameExists;
            }
            else if (Count >= GameConfiguration.MaximumCharacters)
            {
                Packet.Result = CreateCharacterResult.ReachedMaximum;
            }
            else
            {                
                CharacterManager.CreateCharacter(Character);
                Packet.Result = CreateCharacterResult.Successful;

            }
            Client.Socket.Send(Packet);
        }
    }
}