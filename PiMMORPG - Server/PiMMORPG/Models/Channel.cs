using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tFramework.Network.Interfaces;

namespace PiMMORPG.Models
{
    using Enums;

    public class Channel : ModelBase
    {
        public string Name { get; set; }
        public bool IsPVP { get; set; }
        public int Port { get; set; }
        public int Connections { get; set; }
        public int MaximumConnections { get; set; }
        public ServerType Type { get; set; }

        public override void ReadPacket(IDataPacket packet)
        {
            base.ReadPacket(packet);
            Name = packet.ReadString();
            IsPVP = packet.ReadBool();
            Port = packet.ReadInt();
            Connections = packet.ReadInt();
            MaximumConnections = packet.ReadInt();
            Type = packet.ReadEnum<ServerType>();
        }

        public override void WritePacket(IDataPacket packet)
        {
            base.WritePacket(packet);
            packet.WriteString(Name);
            packet.WriteBool(IsPVP);
            packet.WriteInt(Port);
            packet.WriteInt(Connections);
            packet.WriteInt(MaximumConnections);
            packet.WriteEnum(Type);
        }
    }
}