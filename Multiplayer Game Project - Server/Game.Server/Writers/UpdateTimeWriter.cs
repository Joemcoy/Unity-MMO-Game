using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Game.Data;
using Game.Data.Models;
using Server.Configuration;

using Network.Data;
using Network.Data.Interfaces;

namespace Game.Server.Writers
{
    public class UpdateTimeWriter : IRequest
    {
        public DateTime Time { get; set; }

        public uint ID { get { return PacketID.TimeUpdate; } }
        public bool Write(IClientSocket Client, ISocketPacket Packet)
        {
            Packet.WriteDateTime(Time);
            return true;
        }
    }
}
