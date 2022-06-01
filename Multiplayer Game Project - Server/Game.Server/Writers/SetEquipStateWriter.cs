using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Game.Data;
using Network.Data.Interfaces;

namespace Game.Server.Writers
{
    public class SetEquipStateWriter : IRequest
    {
        public uint ID { get { return PacketID.SetEquipState; } }

        public int OwnerID { get; set; }
        public uint InventoryID { get; set; }
        public bool State { get; set; }

        public bool Write(IClientSocket Client, ISocketPacket Packet)
        {
            Packet.WriteInt(OwnerID);
            Packet.WriteUInt(InventoryID);
            Packet.WriteBool(State);

            return true;
        }
    }
}
