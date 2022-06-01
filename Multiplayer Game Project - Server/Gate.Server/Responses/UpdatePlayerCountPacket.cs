using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Gate.Client;
using Network.Data.Interfaces;

namespace Gate.Server.Responses
{
    public class UpdatePlayerCountPacket : GTResponse
    {
        public override uint ID { get { return PacketID.UpdatePlayerCount; } }
        
        bool Increment;

        public override bool Read(ISocketPacket Packet)
        {
            Increment = Packet.ReadBool();

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            if (Increment)
                Client.ClientCount++;
            else
                Client.ClientCount--;
        }
    }
}
