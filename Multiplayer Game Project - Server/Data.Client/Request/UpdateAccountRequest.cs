using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Game.Data.Models;
using Network.Data.Interfaces;

namespace Data.Client.Requests
{
    public class UpdateAccountRequest : IRequest
    {
        public uint ID { get { return PacketID.DataUpdateAccount; } }
        public AccountModel Account { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            Account.WritePacket(Packet);

            return true;
        }
    }
}
