﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Network.Data.Interfaces;

namespace Data.Client.Requests
{
    public class ItemsInWorldRequest : IRequest
    {
        public uint ID { get { return PacketID.DataSendWorldItems; } }
        public int MapID { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            Packet.WriteInt(MapID);
            return true;
        }
    }
}
