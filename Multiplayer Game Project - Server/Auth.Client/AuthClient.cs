using System;

using Base.Data.Interfaces;
using Network.Data.Interfaces;
using Network.Data.Dispatchers;
using Network.Data.EventArgs;

using Network.v1;
using Network.Data.Enums;

using Base.Factories;
using Game.Data.Models;
using Base.Data.Abstracts;
using Network.Bases;

namespace Auth.Client
{
	public class AuthClient : ClientBase<AuthClient>
#if UNITY_BUILD || UNITY_STANDALONE || UNITY_EDITOR
        ,ISingleton
#endif
    {
		public AccountModel Account { get; set; }

#if UNITY_5
        void ISingleton.Create() { }
        void ISingleton.Destroy()
        {
            Local.Scripts.SocketRegister.RemoveSocket(Socket);
            Socket.Disconnect();
        }
#endif
    }
}