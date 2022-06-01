using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Network.Data;using Network.Data.Interfaces;
using Game.Data;
using Game.Data.Models;
using Game.Data.Information;
using Game.Controller;
using Base.Factories;
using Data.Client;


namespace Data.Server.Responses
{
    public class UpdateCharacterPositionPacket : DCResponse
    {
        CharacterModel Character;
        public override uint ID { get { return PacketID.DataUpdateCharacterPosition; } }

        public override bool Read(ISocketPacket Packet)
        {
            Character = new CharacterModel();
            Character.ReadPacket(Packet);

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            CharacterManager.UpdateCharacter(Character);
        }
    }
}