using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Network.Data.Interfaces;

namespace Data.Client.Requests
{
    public class MessagesRequest : IRequest
    {
        public uint ID { get { return PacketID.DataSendMessages; } }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            return true;
        }
    }
}
