using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Network.Interfaces;
namespace PiMMORPG.Server.BattleRoyale.Requests
{
    using Client.BattleRoyale;
    public class ElevateWaterRequest : PiBRRequest
    {
        public override ushort ID => PacketID.ElevateWater;

        public int Level { get; set; }
        public override bool Write(IDataPacket packet)
        {
            packet.WriteInt(Level);
            return true;
        }
    }
}