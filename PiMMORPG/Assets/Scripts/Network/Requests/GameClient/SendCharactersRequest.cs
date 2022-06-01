using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PiMMORPG;
using PiMMORPG.Client;
using PiMMORPG.Client.Auth;

using tFramework.Factories;
using tFramework.Network.Interfaces;

namespace Scripts.Network.Requests.GameClient
{
    public class SendCharactersRequest : PiBaseRequest
    {
        public override ushort ID { get { return PacketID.SendCharacters; } }

        public override bool Write(IDataPacket packet)
        {
            var client = SingletonFactory.GetSingleton<PiAuthClient>();
            packet.WriteUInt(client.User.ID);

            return true;
        }
    }
}