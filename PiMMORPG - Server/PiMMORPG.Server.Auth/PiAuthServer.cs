using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Factories;
using tFramework.Interfaces;

using tFramework.Network;
using tFramework.Network.Enums;
using tFramework.Network.Bases;

namespace PiMMORPG.Server.Auth
{
    using Client.Auth;
    using General;
    using General.Bases;
    using General.Drivers;

    public class PiAuthServer : BaseServer<PiAuthServer, TCPAsyncServer, PiAuthClient, TCPAsyncClient>, ISingleton
    {
        ILogger logger;

        void ISingleton.Created()
        {
            logger = LoggerFactory.GetLogger(this);
            Socket.EndPoint.Port = ServerControl.Configuration.Port;

            RegisterResponses<PiAuthResponse>();
        }

        void ISingleton.Destroyed()
        {

        }

        protected override void Connected(PiAuthClient client)
        {
            base.Connected(client);
            logger.LogInfo("Client {0} has been connected to auth server!", client.Socket.EndPoint);
        }

        protected override void Disconnected(PiAuthClient client, DisconnectReason reason)
        {
            base.Disconnected(client, reason);
            var str = string.Format("Client {0} has been disconnected from auth server ({1})!", client.Socket.EndPoint, reason);

            if (reason != DisconnectReason.Normal)
                logger.LogWarning(str);
            else
                logger.LogInfo(str);
        }
    }
}