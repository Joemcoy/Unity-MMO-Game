using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Network.Interfaces;
using tFramework.Factories;

namespace PiMMORPG.Server.General.Responses
{
    using Client;
    using Requests;

    using General;
    using General.Drivers;

    public class SelectCharacterResponse : PiBaseResponse
    {
        public override ushort ID => PacketID.SelectCharacter;

        uint CharacterID;
        protected SendCharacterRequest packet = new SendCharacterRequest();

        public override bool Read(IDataPacket packet)
        {
            CharacterID = packet.ReadUInt();
            return true;
        }

        protected virtual void LoadCharacter()
        {
            packet.Character = Client.Character = Client.Characters.First(c => c.ID == CharacterID);

            using (var ctx = new CharacterItemDriver())
                packet.Items = Client.Character.Items = ctx.GetModels(ctx.CreateBuilder().Where(m => m.Character).Equal(Client.Character.ID));
        }

        public override void Execute()
        {
            var server = ServerControl.GetServer(Socket.Server.EndPoint.Port);

            if (server.Clients.Any(c => c.Character != null && !c.Equals(Client) && c.Character.ID == CharacterID))
                packet.Result = false;
            else
            {
                Client.SwitchingMap = true;
                packet.Result = true;
                LoadCharacter();

                while (server.Clients.Any(c => c.Character != null && c.Character.ID != packet.Character.ID && c.Character.Position.Distance(packet.Character.Position) < 5))
                    packet.Character.Position.PositionX += 2.5f;

                Socket.Send(packet);
            }
        }
    }
}