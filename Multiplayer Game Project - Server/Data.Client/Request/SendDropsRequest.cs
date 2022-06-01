using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Game.Data.Models;
using Network.Data.Interfaces;

namespace Data.Client.Requests
{
    public class SendDropsRequest : IRequest
    {
        public uint ID { get { return PacketID.DataSendDrops; } }
        public int MapID { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            return true;
        }
    }
}
