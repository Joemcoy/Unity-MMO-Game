using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Network.Data.Interfaces;

namespace Data.Client.Requests
{
    public class AccountRequest : IRequest
    {
        public uint ID { get { return PacketID.DataSendAccountByID; } }
        public int AccountID { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            Packet.WriteInt(AccountID);
            return true;
        }
    }
}
