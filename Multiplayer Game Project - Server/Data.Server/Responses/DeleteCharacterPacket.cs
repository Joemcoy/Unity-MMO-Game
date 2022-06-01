using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Network.Data;
using Game.Data;
using Game.Data.Results;
using Game.Data.Models;
using Game.Controller;
using Data.Client;
using Base.Factories;
using Network.Data.Interfaces;

using Data.Server.Writers;

namespace Data.Server.Responses
{
    public class DeleteCharacterPacket : DCResponse
    {
        public override uint ID { get { return PacketID.DataDeleteCharacter; } }
        int CID;

        public override bool Read(ISocketPacket Packet)
        {
            //AccountID = Packet.ReadInt();
            //GatePort = Packet.ReadInt();
            CID = Packet.ReadInt();

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Packet = new DeleteCharacterRequest();

            CharacterModel Character = CharacterManager.GetCharacterByID(CID);
            if (Character == null)
                Packet.Result = DeleteCharacterResult.NotFound;
            else
            {
                ControllerFactory.GetBaseController("characters").RemoveModel(Character);
                Packet.Result = DeleteCharacterResult.Success;
            }

            Client.Socket.Send(Packet);
        }
    }
}