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
    using General.Interfaces;

    public class RequestMapDataResponse : PiBaseResponse
    {
        public override ushort ID => PacketID.RequestMapData;

        protected IGameServer server;
        protected MapDataRequest packet;

        public override bool Read(IDataPacket packet)
        {
            Client.SwitchingMap = false;

            server = ServerControl.GetServer(Socket.Server.EndPoint.Port);
            this.packet = new MapDataRequest();
            return true;
        }

        public override void Execute()
        {            
            Socket.Send(packet);            
        }
    }
}