using System;
using System.Collections.Generic;
using System.Net;

using Base.Data.Interfaces;
using Network.Data.Interfaces;

using Base.Factories;
using Gate.Server;
using Auth.Client;

using Network.v1;
using Network.Data.Dispatchers;

using Server.Configuration;
using Gate.Client;
using Data.Client;
using Game.Data.Models;
using Network.Data.EventArgs;
using Base.Configurations;
using Network.Bases;
using System.Reflection;

namespace Auth.Server
{
	public class AuthServer : ServerBase<AuthClient>, ISingleton, IComponent
	{
        public LauncherFileModel[] Files { get; set; }
        protected override Assembly ResponsesAssembly { get { return typeof(AuthServer).Assembly; } }

        public bool Enable()
		{
			try
            {
                DataClient Data = SingletonFactory.GetInstance<DataClient>();
                GateServer Gate = SingletonFactory.GetInstance<GateServer>();

                if (!ComponentFactory.Enable<IntervalConfiguration>())
                    return false;
                else if (!ComponentFactory.Enable<PortsConfiguration>())
					return false;
                else if (!ComponentFactory.Enable<GatesConfiguration>())
                    return false;
                else if(!ComponentFactory.Enable<DataClient>())
					return false;
				else if(!ComponentFactory.Enable<GateServer>())
					return false;
				else
				{
                    Data.Socket.RegisterResponse<DCResponse>();
                    Data.Socket.IOEnabled = true;

                    Socket.EndPoint.Port = PortsConfiguration.AuthPort;

                    if (Socket.Open())
                    {
                        Data.SendLauncherFilesRequest(this);
                        LoggerFactory.GetLogger(this).LogWarning("Waiting for the launcher files...");

                        return true;
                    }
                    else
                        return false;
				}
			}
			catch(Exception ex)
			{
				LoggerFactory.GetLogger(this).LogFatal(ex);
				return false;
			}
		}

		public bool Disable()
		{
			try
			{
                Socket.Close();

				return true;
			}
			catch(Exception ex)
			{
                LoggerFactory.GetLogger(this).LogFatal(ex);
				return false;
			}
		}

        void ISingleton.Create() { }
        void ISingleton.Destroy() { }
    }
}
