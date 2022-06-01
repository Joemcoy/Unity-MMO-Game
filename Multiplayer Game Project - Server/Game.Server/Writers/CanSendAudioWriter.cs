using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Network.Data.Interfaces;

namespace Game.Server.Writers
{
    public class CanSendAudioWriter : IRequest
    {
        public uint ID { get { return PacketID.CanSendAudio; } }
        public bool CanSend { get; set; }
        public int RemainSeconds { get; set; }

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            Packet.WriteBool(CanSend);

            if (!CanSend)
                Packet.WriteInt(RemainSeconds);

            return true;
        }
    }
}