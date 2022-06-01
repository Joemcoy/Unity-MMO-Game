using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Network.Data;using Network.Data.Interfaces;
using Gate.Client;

namespace Chat.Server.GateResponses
{
    public class AuthPacket : GTResponse
    {
        string Username;
        int AccountID;
        bool AuthMode;

        public override uint ID { get { return PacketID.AuthClient; } }
        public override bool Read(ISocketPacket Packet)
        {
            Username = Packet.ReadString();
            AccountID = Packet.ReadInt();
            AuthMode = Packet.ReadBool();
            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            if (AuthMode && !Client.Usernames.Contains(Username))
                Client.Usernames.Add(Username);
            else if (!AuthMode && Client.Usernames.Contains(Username))
                Client.Usernames.Remove(Username);
        }
    }
}
