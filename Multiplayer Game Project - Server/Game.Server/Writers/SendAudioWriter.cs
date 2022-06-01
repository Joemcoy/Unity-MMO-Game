using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Network.Data.Interfaces;

namespace Game.Server.Writers
{
    public class SendAudioWriter : IRequest
    {
        public uint ID { get { return PacketID.SendAudio; } }

        public int CID { get; set; }
        public int Channels { get; set; }
        public int Frequency { get; set; }
        public byte[] Buffer { get; set; }        

        public bool Write(IClientSocket Socket, ISocketPacket Packet)
        {
            Packet.WriteInt(CID);
            Packet.WriteInt(Channels);
            Packet.WriteInt(Frequency);
            Packet.WriteBuffer(Buffer);
            return true;
        }
    }
}
