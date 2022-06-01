using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Client;
using Network.Data;
using Network.Data.Interfaces;
using Game.Data;
using Data.Client;
using Base.Factories;
using Game.Data.Models;
using Server.Configuration;
using Game.Server.Writers;
using Game.Data.Results;

namespace Game.Server.Responses
{
    public class CreateCharacterPacket : GCResponse
    {
        string Name;
        bool IsFemale;
        CharacterStyleModel Style;

        public override uint ID { get { return PacketID.CreateCharacter; } }
        public override bool Read(ISocketPacket Packet)
        {
            Name = Packet.ReadString();
            IsFemale = Packet.ReadBool();

            Style = new CharacterStyleModel();
            Style.ReadPacket(Packet);

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            LoggerFactory.GetLogger(this).LogInfo("Creating character {0}...", Name);
            if (Client.Characters.Length < GameConfiguration.MaximumCharacters)
            {
                DataClient RClient = SingletonFactory.GetInstance<DataClient>();

                CharacterModel Character = new CharacterModel();
                Character.AID = this.Client.Account.ID;
                Character.Name = Name;
                Character.IsFemale = IsFemale;
                Character.Style = Style;

                RClient.SendCreateCharacterRequest(this.Client, Character);
            }
            else
            {
                CreateCharacterWriter Packet = new CreateCharacterWriter();
                Packet.Result = CreateCharacterResult.NameExists;

                Client.Socket.Send(Packet);
            }
        }
    }
}
