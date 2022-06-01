using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Game.Data.Models;
using Network.Data.Interfaces;

namespace Data.Server.Writers
{
    public class MapsRequest : IRequest
    {
        public uint ID { get { return PacketID.DataSendMaps; } }
        public MapModel[] Maps { get; set; }

        public bool Write(IClientSocket Client, ISocketPacket Packet)
        {
            Packet.WriteInt(Maps.Length);
            foreach (var Map in Maps)
                Map.WritePacket(Packet);

            return true;
        }
    }
}
