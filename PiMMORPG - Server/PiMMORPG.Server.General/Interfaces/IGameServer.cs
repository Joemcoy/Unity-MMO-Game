using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Network;
using tFramework.Interfaces;
using tFramework.Network.Interfaces;

namespace PiMMORPG.Server.General.Interfaces
{
    using Models;
    using Client.Interfaces;

    public interface IGameServer : IComponent
    {
        Channel Channel { get; set; }
        IGameClient[] Clients { get; }
        TCPAsyncServer Socket { get; }

        void SendSystemMessage(string message, Predicate<IGameClient> condition = null);
        void SendToAll(IRequest<TCPAsyncClient> packet, Predicate<IGameClient> condition = null);
    }
}