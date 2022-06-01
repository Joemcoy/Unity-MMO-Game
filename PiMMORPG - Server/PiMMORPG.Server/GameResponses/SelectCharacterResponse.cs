using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Network.Interfaces;
using tFramework.Factories;

namespace PiMMORPG.Server.GameResponses
{
    using Client;
    using PiMMORPG.Server.Drivers;
    using PiMMORPG.Server.GameRequests;

    public class SelectCharacterResponse : PiGameResponse
    {
        public override ushort ID => PacketID.SelectCharacter;

        uint CharacterID;
        public override bool Read(IDataPacket packet)
        {
            CharacterID = packet.ReadUInt();
            return true;
        }

        public override void Execute()
        {
            var packet = new SendCharacterRequest();
            var server = SingletonFactory.GetSingleton<PiServer>();
            var gameServer = server.Channels.First(c => c.Channel.Port == Socket.Server.EndPoint.Port);

            if (gameServer.Clients.Any(c => c.Character != null && !c.Equals(Client) && c.Character.ID == CharacterID))
            {
                packet.Result = false;
            }
            else
            {
                Client.SwitchingMap = true;
                packet.Result = true;
                packet.Character = Client.Character = Client.Characters.First(c => c.ID == CharacterID);

                using (var ctx = new CharacterItemDriver())
                    packet.Items = Client.Character.Items = ctx.GetModels(ctx.CreateBuilder().Where(m => m.Character).Equal(Client.Character.ID));

                while (gameServer.Clients.Any(c => c.Character != null && c.Character.ID != packet.Character.ID && c.Character.Position.Distance(packet.Character.Position) < 5))
                    packet.Character.Position.PositionX += 2.5f;

                Socket.Send(packet);
            }
        }
    }
}