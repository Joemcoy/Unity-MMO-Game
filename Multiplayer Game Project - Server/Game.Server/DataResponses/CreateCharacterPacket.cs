using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Network.Data;
using Network.Data.Interfaces;

using Game.Client;
using Data.Client;
using Game.Server.Writers;

namespace Game.Server.DataResponses
{
    public class CreateCharacterPacket : DCResponse
    {
        byte Result;
        public override uint ID { get { return PacketID.DataCreateCharacter; } }

        public override bool Read(ISocketPacket Packet)
        {
            Result = Packet.ReadByte();

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            GameClient Client = this.Client.Dequeue<GameClient>();

            CreateCharacterWriter Packet = new CreateCharacterWriter();
            Packet.Result = Result;

            Client.Socket.Send(Packet);
        }
    }
}
