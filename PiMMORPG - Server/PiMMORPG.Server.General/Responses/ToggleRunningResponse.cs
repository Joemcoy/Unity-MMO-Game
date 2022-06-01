using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tFramework.Factories;
using tFramework.Network.Interfaces;

namespace PiMMORPG.Server.General.Responses
{
    using Client;
    using Requests;
    using General;

    public class ToggleRunningResponse : PiBaseResponse
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

            var server = ServerControl.GetServer(Socket.Server.EndPoint.Port);
            foreach (var client in server.Clients.Where(c => c.Character != null && !c.Equals(Client)))
            {
                client.Socket.Send(packet);
            }
        }
    }
}