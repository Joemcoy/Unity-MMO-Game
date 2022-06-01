using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Network.Interfaces;
using tFramework.Factories;

namespace PiMMORPG.Server.General.Responses
{
    using Client;
    using Models;
    using Requests;
    using General;

    public class SyncCharacterResponse : PiBaseResponse
    {
        public override ushort ID => PacketID.SyncCharacter;
        
        Position position;
        float h, v;

        public override bool Read(IDataPacket packet)
        {
            position = new Position();
            position.ReadPacket(packet);

            h = packet.ReadFloat();
            v = packet.ReadFloat();
            return true;
        }

        public override void Execute()
        {
            Client.Character.Position.Copy(position);
            var packet = new SyncCharacterRequest();
            packet.CharacterId = Client.Character.ID;
            packet.Position = position;
            packet.Horizontal = h;
            packet.Vertical = v;

            var server = ServerControl.GetServer(Socket.Server.EndPoint.Port);
            foreach (var client in server.Clients.Where(c => c.CanSpawn(Client, false)))
            {
                client.Socket.Send(packet);
            }
        }
    }
}