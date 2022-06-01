using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Gate.Client;
using Network.Data;
using Game.Data;
using Base.Factories;
using Network.Data.Interfaces;

namespace Gate.Server.Responses
{
    public class AuthPacket : GTResponse
    {
        string Username;
        int AID;

        public override uint ID { get { return PacketID.AuthClient; } }

        public override bool Read(ISocketPacket Packet)
        {
            Username = Packet.ReadString();
            AID = Packet.ReadInt();

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            foreach (GateClient Client in Client.Server.Clients.Where(c => c.Type != Game.Data.Enums.GateType.Unknown && c.Socket.EndPoint != Client.Socket.EndPoint))
            {
                //Client.SendAuthorization(Username, AID);
            }
        }
    }
}