using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Base.Data.Interfaces;
using Game.Data.Enums;
using Network.Data;
using Network.Data.Interfaces;
using Game.Data.Attributes;
using Game.Data.Abstracts;

namespace Game.Data.Models
{
    public class MessageModel : APacketWrapper, IModel
    {
        public int ID { get; set; }
        public string Username { get; set; }

        [ColumnData(Name = "Message")]
        public string Content { get; set; }

        public AccessLevel Access { get; set; }
        public MessageType Type { get; set; }
        public DateTime SentTime { get; set; }

        [NonColumn]
        public string Arguments { get; set; }

        public override void ReadPacket(ISocketPacket Packet)
        {
            base.ReadPacket(Packet);

            Username = Username.Replace('<', ' ').Replace('>', ' ');
            Content = Content.Replace('<', ' ').Replace('>', ' ');
        }

        public override void WritePacket(ISocketPacket Packet)
        {
            Username = Username.Replace('<', ' ').Replace('>', ' ');
            Content = Content.Replace('<', ' ').Replace('>', ' ');

            base.WritePacket(Packet);
        }
    }
}
