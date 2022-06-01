using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PiMMORPG;
using PiMMORPG.Client;
using tFramework.Network.Interfaces;

namespace Scripts.Network.Requests.GameClient
{
    public class ToggleRunningRequest : PiBaseRequest
    {
        public override ushort ID { get { return PacketID.ToggleRunning; } }
        public bool Running { get; set; }
        public override bool Write(IDataPacket packet)
        {
            packet.WriteBool(Running);
            return true;
        }
    }
}
