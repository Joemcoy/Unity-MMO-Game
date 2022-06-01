using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Network.Interfaces;
namespace PiMMORPG.Server.General.Requests
{
    using Client;

    public class UpdateTimeRequest : PiBaseRequest
    {
        public override ushort ID => PacketID.UpdateTime;

        public TimeSpan Time { get; set; }
        public override bool Write(IDataPacket packet)
        {
            packet.WriteTimeSpan(Time);
            return true;
        }
    }
}