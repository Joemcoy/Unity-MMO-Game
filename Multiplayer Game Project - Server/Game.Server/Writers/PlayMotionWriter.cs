using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Network.Data.Interfaces;

namespace Game.Server.Writers
{
    public class PlayMotionWriter : IRequest
    {
        public uint ID { get { return PacketID.PlayMotion; } }

        public int CID { get; set; }
        public string TriggerName { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            Packet.WriteInt(CID);
            Packet.WriteString(TriggerName);
            return true;
        }
    }
}
