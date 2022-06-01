using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tFramework.Data.Interfaces;

namespace PiMMORPG.Server.General.Requests
{
    using Client;
    using Models;
    using tFramework.Network.Interfaces;

    public class MapDataRequest : PiBaseRequest
    {
        public Character[] Characters { get; set; }
        public Drop[] Drops { get; set; }
        public Tree[] Trees { get; set; }

        public override ushort ID => PacketID.RequestMapData;
        public override bool Write(IDataPacket packet)
        {
            packet.WriteInt(Characters.Length);
            foreach(var character in Characters)
            {
                packet.WriteWrapper(character);
                packet.WriteWrappers(character.Items.Where(e => e.Equipped).ToArray());
            }

            packet.WriteWrappers(Drops);
            packet.WriteWrappers(Trees);
            return true;
        }
    }
}