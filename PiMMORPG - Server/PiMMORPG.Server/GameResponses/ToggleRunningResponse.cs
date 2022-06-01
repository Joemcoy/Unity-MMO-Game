using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tFramework.Factories;
using tFramework.Network.Interfaces;

namespace PiMMORPG.Server.GameResponses
{
    using Client;
    using GameRequests;

    public class ToggleRunningResponse : PiRPGResponse
    {
        public override ushort ID => PacketID.ToggleRunning;

        bool Running;
        public override bool Read(IDataPacket packet)
        {
            Running = packet.ReadBool();
            return true;
        }

        public override void Execute()
        {
            var packet = new ToggleRunningRequest();
            packet.CID = Client.Character.ID;
            packet.Running = Running;

            var server = SingletonFactory.GetSingleton<PiServer>().Channels.First(s => s.Channel.Port == Socket.Server.EndPoint.Port);
            foreach (var client in server.Clients.Where(c => c.Character != null && !c.Equals(Client)))
            {
                client.Socket.Send(packet);
            }
        }
    }
}