using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Game.Data.Models;
using Network.Data.Interfaces;

namespace Data.Client.Requests
{
    public class AddDropRequest : IRequest
    {
        public uint ID { get { return PacketID.DataAddDrop; } }
        public DropModel Drop { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            Drop.WritePacket(Packet);

            return true;
        }
    }
}
