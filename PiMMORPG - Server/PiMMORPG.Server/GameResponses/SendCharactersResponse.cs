using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Network.Interfaces;
using tFramework.Extensions;

namespace PiMMORPG.Server.GameResponses
{
    using Client;
    using PiMMORPG.Server.Drivers;
    using PiMMORPG.Server.GameRequests;

    public class SendCharactersResponse : PiRPGResponse
    {
        public override ushort ID => PacketID.SendCharacters;

        uint aid;
        public override bool Read(IDataPacket packet)
        {
            aid = packet.ReadUInt();
            return true;
        }

        public override void Execute()
        {
            var packet = new SendCharactersRequest();
            using (var ctx = new AccountDriver())
                Client.Account = ctx.GetModel(ctx.CreateBuilder().Where(m => m.ID).Equal(aid));

            using (var ctx = new CharacterDriver())
                packet.Characters = Client.Characters = ctx.GetModels(ctx.CreateBuilder().Where(m => m.Account).Equal(Client.Account));
            Socket.Send(packet);
        }
    }
}