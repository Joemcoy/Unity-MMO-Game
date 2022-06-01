using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Base.Data.Interfaces;
using Network.Data.Interfaces;

namespace Network.Data.Dispatchers
{
    public interface IServerSocketDispatcher : IDispatcherBase
    {
        void OnOpen(IServerSocket Server);
        void OnClose(IServerSocket Server);

        void OnClientConnected(IServerSocket Server, IClientSocket Socket);
        void OnClientDisconnected(IServerSocket Server, IClientSocket Socket);

        void OnError(IServerSocket Server, Exception ex);
    }
}
