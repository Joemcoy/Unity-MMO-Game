using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Network;
using tFramework.Network.Bases;
using tFramework.Network.Enums;
using tFramework.Network.Interfaces;

using tFramework.Interfaces;
using tFramework.Factories;

namespace PiMMORPG.Server.General.Bases
{
    using Models;
    using Client;
    using Requests;
    using Interfaces;
    using Client.Interfaces;
    using System.Threading;

    public abstract class GameServerBase<TServer, TClient>
       : BaseServer<TServer, TCPAsyncServer, PiBaseClient, TCPAsyncClient>, IComponent, IGameServer
        where TServer : BaseServer<TServer, TCPAsyncServer, PiBaseClient, TCPAsyncClient>, new()
        where TClient : PiBaseClient
    {
        public Channel Channel { get; set; }
        public new TClient[] Clients { get { return base.Clients.OfType<TClient>().ToArray(); } }
        IGameClient[] IGameServer.Clients => Clients.OfType<IGameClient>().ToArray();

        protected ILogger logger { get; private set; }

        public GameServerBase() { logger = LoggerFactory.GetLogger(this); }
        protected virtual void Connected(TClient client) { }
        protected virtual void Disconnected(TClient client, DisconnectReason reason) { }

        protected override PiBaseClient CreateClientInstance()
        {
            return Activator.CreateInstance<TClient>() as TClient;
        }

        public void SendSystemMessage(string message, Predicate<PiBaseClient> condition = null)
        {
            var packet = new ChatRequest
            { Message = "<b><color=#B200D6>SERVER</color></b>: " + message };

            foreach (var client in Clients.Where(c => condition == null ? true : condition(c)))
                client.Socket.Send(packet);
        }

        bool IComponent.Enable()
        {
            logger.LogSuccess("Server ({0}):{1}:{2} has been opened!", Channel.Name, Channel.Type, Channel.Port);

            Socket.EndPoint.Port = Channel.Port;
            return Socket.Open();
        }

        bool IComponent.Disable()
        {
            return Socket.Close();
        }

        void IGameServer.SendSystemMessage(string message, Predicate<IGameClient> condition)
        {
            SendSystemMessage(message, c => condition == null ? true : condition(c));
        }

        void IGameServer.SendToAll(IRequest<TCPAsyncClient> packet, Predicate<IGameClient> condition)
        {
            SendToAll(packet, c => condition == null ? true : condition(c));
        }

        protected override void Connected(PiBaseClient client)
        {
            base.Connected(client);
            Channel.Connections++;
            Connected(client as TClient);
            logger.LogInfo("Client {0} has been connected to server {1}!", client.Socket.EndPoint, Channel.Name);
        }

        protected override void Disconnected(PiBaseClient client, DisconnectReason reason)
        {
            base.Disconnected(client, reason);
            Channel.Connections--;
            Disconnected(client as TClient, reason);
            var str = string.Format("Client {0} has been disconnected from server {1} ({2})!", client.Socket.EndPoint, Channel.Name, reason);

            if (reason != DisconnectReason.Normal)
                logger.LogWarning(str);
            else
                logger.LogInfo(str);
        }
    }
}