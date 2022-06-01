using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Game.Data.Models;
using Network.Data.Interfaces;

namespace Data.Server.Writers
{
    public class AccountRequest : IRequest
    {
        public uint ID { get { return PacketID.DataSendAccountByID; } }
        public AccountModel Account { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            if (Account == null)
                Packet.WriteBool(false);
            else
            {
                Packet.WriteBool(true);
                Account.WritePacket(Packet);
            }
            return true;
        }
    }
}
