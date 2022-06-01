using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Network.Data.Interfaces;

namespace Data.Client.Requests
{
    public class AccountsRequest : IRequest
    {
        public uint ID { get { return PacketID.DataSendAccounts; } }
        public uint Maximum { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            if (Maximum > 0)
            {
                Packet.WriteBool(true);
                Packet.WriteUInt(Maximum);
            }
            else
                Packet.WriteBool(false);
            return true;
        }
    }
}
