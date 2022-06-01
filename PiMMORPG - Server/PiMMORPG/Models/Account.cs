using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tFramework.Network.Interfaces;

namespace PiMMORPG.Models
{
    public class Account : SerialModelBase
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
        public bool IsBanned { get; set; }
        public AccessLevel Access { get; set; }

        public override void ReadPacket(IDataPacket packet)
        {
            base.ReadPacket(packet);
            Username = packet.ReadString();
            Password = packet.ReadString();
            Nickname = packet.ReadString();
            IsBanned = packet.ReadBool();
            Access = packet.ReadWrapper<AccessLevel>();
        }

        public override void WritePacket(IDataPacket packet)
        {
            base.WritePacket(packet);
            packet.WriteString(Username);
            packet.WriteString(Password);
            packet.WriteString(Nickname);
            packet.WriteBool(IsBanned);
            packet.WriteWrapper(Access);
        }
    }
}
