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
    public class SetWeaponWriter : IRequest
    {
        public int CID { get; set; }
        public int Index { get; set; }

        public uint ID { get { return PacketID.SetWeapon; } }
        public bool Write(IClientSocket Client, ISocketPacket Packet)
        {
            Packet.WriteInt(CID);
            Packet.WriteInt(Index);

            return true;
        }
    }
}
