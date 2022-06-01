using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Data.Interfaces;
using tFramework.Network;
using tFramework.Network.Interfaces;

namespace PiMMORPG.Models
{
    public class Map : ModelBase
    {
        public string Name { get; set; }
        public string Message { get; set; }
        public string SceneName { get; set; }

        public Position Spawn { get; set; }

        public override void ReadPacket(IDataPacket packet)
        {
            base.ReadPacket(packet);
            Name = packet.ReadString();
            Message = packet.ReadString();
            SceneName = packet.ReadString();

            Spawn = packet.ReadWrapper<Position>();
        }

        public override void WritePacket(IDataPacket packet)
        {
            base.WritePacket(packet);
            packet.WriteString(Name);
            packet.WriteString(Message);
            packet.WriteString(SceneName);

            packet.WriteWrapper(Spawn);
        }
    }
}