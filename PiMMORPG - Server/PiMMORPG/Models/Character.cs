using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tFramework.Network.Interfaces;

namespace PiMMORPG.Models
{
    public class Character : ModelBase
    {
        public string Name { get; set; }
        public bool IsFemale { get; set; }

        public Account Account { get; set; }
        public Position Position { get; set; }
        public CharacterStyle Style { get; set; }
        public Map Map { get; set; }
        public CharacterItem[] Items { get; set; }

        public Character()
        {
            Position = new Position();
        }

        public override void ReadPacket(IDataPacket packet)
        {
            base.ReadPacket(packet);
            Name = packet.ReadString();
            IsFemale = packet.ReadBool();

            Account = packet.ReadWrapper<Account>();
            Position = packet.ReadWrapper<Position>();
            Style = packet.ReadWrapper<CharacterStyle>();
            Map = packet.ReadWrapper<Map>();
        }

        public override void WritePacket(IDataPacket packet)
        {
            base.WritePacket(packet);
            packet.WriteString(Name);
            packet.WriteBool(IsFemale);
            packet.WriteWrapper(Account);
            packet.WriteWrapper(Position);
            packet.WriteWrapper(Style);
            packet.WriteWrapper(Map);
        }
    }
}
