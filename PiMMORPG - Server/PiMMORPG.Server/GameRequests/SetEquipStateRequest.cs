using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using tFramework.Network.Interfaces;

namespace PiMMORPG.Server.General.Requests
{
    using Client;
    using Models;

    public class SetEquipStateRequest : PiBaseRequest
    {
        public override ushort ID => PacketID.SetEquipState;

        public uint OwnerID { get; set; }
        public bool ToOwner { get; set; }
        public CharacterItem Equip { get; set; }

        public override bool Write(IDataPacket packet)
        {
            packet.WriteUInt(OwnerID);
            if (ToOwner)
                packet.WriteGuid(Equip.Serial);
            else
                packet.WriteUInt(Equip.Info.InventoryID);
            packet.WriteBool(Equip.Equipped);
            return true;
        }
    }
}