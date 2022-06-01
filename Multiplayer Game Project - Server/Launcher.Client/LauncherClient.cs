using System;

using Network.Data;
using Network.Data.Interfaces;
using Network.v1;

using Base.Data.Enums;
using Game.Data;
using Game.Data.Interfaces;
using Base.Factories;
using Game.Data.Enums;
using Base.Data.Interfaces;
using Game.Data.Models;
using Network.Data.Enums;
using Network.Data.Dispatchers;

namespace Launcher.Client
{
    public class LauncherClient
#if UNITY_BUILD || UNITY_STANDALONE || UNITY_EDITOR
        : ISingleton, IComponent
#endif
    {
        public ClientSocket Socket { get; private set; }
        public AccountModel Account { get; set; }

        public LauncherClient() : this(new ClientSocket())
        {
        }

        public LauncherClient(ClientSocket Socket)
        {
            this.Socket = Socket;
            this.Socket.RegisterResponse<LCReader>();
        }

        public void Create()
        {

        }

        public void Destroy()
        {
            Socket.Disconnect();
        }
    

        public void OnError(IClientSocket Client, Exception Error)
        {
            LoggerFactory.GetLogger(this).LogError("Server has caught a exception!");
            LoggerFactory.GetLogger(this).LogFatal(Error);
        }

        public void OnDispatch(Action Method)
        {
#if UNITY_5
            Local.Helper.SafeInvoker.Invoke(Method);
#else
            Method();
#endif
        }

#if UNITY_BUILD || UNITY_STANDALONE || UNITY_EDITOR
        public bool Enable()
        {
            try
            {
                Socket.Connect();

                return true;
            }
            catch (Exception ex)
            {
                LoggerFactory.GetLogger(this).LogFatal(ex);
                return false;
            }
        }

        public bool Disable()
        {
            try
            {
                Socket.Disconnect();
                return true;
            }
            catch (Exception ex)
            {
                LoggerFactory.GetLogger(this).LogFatal(ex);
                return false;
            }
        }
#endif
    }
}