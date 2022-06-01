using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Network.Data.Interfaces;

namespace Game.Server.Writers
{
    public class ChangeMapWriter : IRequest
    {
        public uint ID { get { return PacketID.ChangeMap; } }
        public string SceneID { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            Packet.WriteString(SceneID);
            return true;
        }
    }
}
