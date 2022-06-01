using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Data.Client;
using Auth.Client;

using Network.Data;
using Network.Data.Interfaces;

using Game.Data;
using Auth.Server.Requests;
using Game.Data.Results;

namespace Auth.Server.DataResponses
{
    public class RegisterPacket : DCResponse
    {
        RegisterResult Result;

        public override uint ID { get { return PacketID.DataSendRegister; } }
        public override bool Read(ISocketPacket Packet)
        {
            Result = Packet.ReadEnum<RegisterResult>();
            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            AuthClient Client = this.Client.Dequeue<AuthClient>();

            var Packet = new RegisterResultRequest();
            Packet.Result = Result;

            Client.Socket.Send(Packet);
        }
    }
}
