using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiMMORPG.Server.General.Requests
{
    using Client;
    using tFramework.Network.Interfaces;

    public class ToggleRunningRequest : PiBaseRequest
    {
        public override ushort ID => PacketID.ToggleRunning;

        public uint CID { get; set; }
        public bool Running { get; set; }

        public override bool Write(IDataPacket packet)
        {
            packet.WriteUInt(CID);
            packet.WriteBool(Running);
            return true;
        }
    }
}