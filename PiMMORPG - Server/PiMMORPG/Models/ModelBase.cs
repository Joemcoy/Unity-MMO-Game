using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Network;
using tFramework.Data.Interfaces;
using tFramework.Network.Interfaces;

namespace PiMMORPG.Models
{
    public abstract class ModelBase : APacketWrapper, IModel
    {
        public uint ID { get; set; }

        public override void ReadPacket(IDataPacket packet)
        {
            ID = packet.ReadUInt();
        }

        public override void WritePacket(IDataPacket packet)
        {
            packet.WriteUInt(ID);
        }
    }

    public abstract class SerialModelBase : ModelBase, ISerialModel
    {
        public Guid Serial { get; set; }

        public override void ReadPacket(IDataPacket packet)
        {
            base.ReadPacket(packet);
            Serial = packet.ReadGuid();
        }

        public override void WritePacket(IDataPacket packet)
        {
            base.WritePacket(packet);
            packet.WriteGuid(Serial);
        }
    }
}