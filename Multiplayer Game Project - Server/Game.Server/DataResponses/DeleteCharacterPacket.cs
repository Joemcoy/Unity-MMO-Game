using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Data.Client;
using Game.Client;
using Game.Data;
using Network.Data;
using Network.Data.Interfaces;
using Game.Server.Writers;

namespace Game.Server.DataResponses
{
    public class DeleteCharacterPacket : DCResponse
    {
        byte Result;
        public override uint ID { get { return PacketID.DataDeleteCharacter; } }
        public override bool Read(ISocketPacket Packet)
        {
            Result = Packet.ReadByte();
            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            GameClient Client = this.Client.Dequeue<GameClient>();

            DeleteCharacterWriter Packet = new DeleteCharacterWriter();
            Packet.Result = Result;

            Client.Socket.Send(Packet);
        }
    }
}
