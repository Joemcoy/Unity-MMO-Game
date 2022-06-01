using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Network.Data;
using Network.Data.Interfaces;
using Game.Data;
using Game.Data.Models;
using Game.Data.Results;

using Game.Controller;
using Data.Client;

using Data.Server.Writers;

namespace Data.Server.Responses
{
    public class SendCharactersPacket : DCResponse
    {
        int AccountID;

        public override uint ID { get { return PacketID.DataSendCharacters; } }
        public override bool Read(ISocketPacket Packet)
        {
            AccountID = Packet.ReadInt();

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Packet = new CharacterListRequest();
            Packet.Characters = CharacterManager.GetCharactersByAID(AccountID);

            Client.Socket.Send(Packet);
        }
    }
}