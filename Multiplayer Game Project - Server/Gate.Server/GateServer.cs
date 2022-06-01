using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using Base.Data.Interfaces;
using Base.Factories;

using Network.v1;
using Network.Data.Interfaces;
using Network.Data.Dispatchers;
using Network.Data.EventArgs;

using Gate.Client;
using Server.Configuration;
using System.Reflection;
using Network.Bases;

namespace Gate.Server
{
    public class GateServer : ServerBase<GateClient>, ISingleton, IComponent
    {
        public int Port = -1;
        public Assembly CurrentAssembly { get; set; }

        protected override Assembly ResponsesAssembly { get { return CurrentAssembly == null ? typeof(GateServer).Assembly : CurrentAssembly; } }

        void ISingleton.Create() { }
        void ISingleton.Destroy() { }

        #region IComponent implementation
        public bool Enable()
        {
            try
            {
                if (!ComponentFactory.Enable<PortsConfiguration>())
                    return false;

                Socket.EndPoint = new IPEndPoint(IPAddress.Any, Port <= 0 ? PortsConfiguration.GatePort : Port);
                return Socket.Open();
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
                Socket.Close();
                return true;
            }
            catch (Exception ex)
            {
                LoggerFactory.GetLogger(this).LogFatal(ex);
                return false;
            }
        }
        #endregion
    }
}