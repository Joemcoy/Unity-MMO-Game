using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tFramework.Network.Interfaces;

namespace PiMMORPG.Models
{
    public class APIAccess : SerialModelBase
    {
        public bool CanRegister { get; set; }
        public bool CanListAccounts { get; set; }
        public bool CanListChannels { get; set; }
        public bool CanListAccessLevels { get; set; }
        public bool CanListCharacters { get; set; }
        public bool CanListDrops { get; set; }
        public bool CanListTrees { get; set; }
        public bool CanListMaps { get; set; }
        public bool CanListMapSpawns { get; set; }
        public bool CanListItemTypes { get; set; }
        public bool CanListAPIAccess { get; set; }

        public override void WritePacket(IDataPacket packet)
        {
            base.WritePacket(packet);
            packet.WriteBool(CanRegister);
            packet.WriteBool(CanListAccounts);
            packet.WriteBool(CanListChannels);
            packet.WriteBool(CanListAccessLevels);
            packet.WriteBool(CanListCharacters);
            packet.WriteBool(CanListDrops);
            packet.WriteBool(CanListTrees);
            packet.WriteBool(CanListMaps);
            packet.WriteBool(CanListMapSpawns);
            packet.WriteBool(CanListItemTypes);
            packet.WriteBool(CanListAPIAccess);
        }

        public override void ReadPacket(IDataPacket packet)
        {
            base.ReadPacket(packet);
            CanRegister = packet.ReadBool();
            CanListAccounts = packet.ReadBool();
            CanListChannels = packet.ReadBool();
            CanListAccessLevels = packet.ReadBool();
            CanListCharacters = packet.ReadBool();
            CanListDrops = packet.ReadBool();
            CanListTrees = packet.ReadBool();
            CanListMaps = packet.ReadBool();
            CanListMapSpawns = packet.ReadBool();
            CanListItemTypes = packet.ReadBool();
            CanListAPIAccess = packet.ReadBool();
        }
    }
}