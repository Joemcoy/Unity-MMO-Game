using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Network.Data.Interfaces;

namespace Gate.Client.Responses.Writers
{
    public class UpdatePlayerCountWriter : IRequest
    {
        public uint ID { get { return PacketID.UpdatePlayerCount; } }
        public bool Increment { get; set; }

        public bool Write(IClientSocket Client, ISocketPacket Packet)
        {
            Packet.WriteBool(Increment);

            return true;
        }
    }
}
