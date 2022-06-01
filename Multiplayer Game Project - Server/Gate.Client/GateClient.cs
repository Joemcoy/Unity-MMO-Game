using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

using Network.Data;
using Network.Data.Enums;
using Network.Data.Interfaces;
using Network.Data.EventArgs;

using Network.v1;

using Game.Data;
using Game.Data.Enums;
using Game.Data.Information;

using Base.Data.Interfaces;
using Base.Factories;

using Server.Configuration;
using Network.Bases;

namespace Gate.Client
{
    public class GateClient : ClientBase<GateClient>, IComponent, ISingleton
    {
        public GateType Type { get; set; }
        public int Port { get; set; }
        public int GatePort { get; set; }
        public string Name { get; set; }
        public bool PVP { get; set; }
        public int MaximumClients { get; set; }
        public int ClientCount { get; set; }
        public string Address { get; set; }

        public GateInfo Info
        {
            get
            {
                return new GateInfo(Port, Name, PVP)
                {
                    MaximumClients = MaximumClients,
                    ClientCount = ClientCount,
                    Address = Address
                };
            }
            set
            {
                Port = value.Port;
                Name = value.Name;
                PVP = value.PVP;
                MaximumClients = value.MaximumClients;
                Address = value.Address;
            }
        }
        public List<string> Usernames { get; private set; }

        public static string[] AuthorizedUsernames
        {
            get
            {
                GateClient Client = SingletonFactory.GetInstance<GateClient>();
                return Client.Usernames.ToArray();
            }
        }

        public bool Enable()
        {
            try
            {
                if (!ComponentFactory.Enable<PortsConfiguration>())
                    return false;

                Socket.EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), GatePort <= 0 ? PortsConfiguration.GatePort : GatePort);

                if (Socket.Connect())
                {
                    LoggerFactory.GetLogger(this).LogInfo($"Gate {Socket.EndPoint.Port} has been opened as {Type}!");
                    if(Type == GateType.Game)
                    {
                        LoggerFactory.GetLogger(this).LogInfo($"Game Gate | {Name} - {(PVP ? "PVP" : "PVE")}");
                    }
                    return true;
                }
                else
                    return false;
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
        
        public void Create()
        {
            
        }

        public void Destroy()
        {

        }
    }
}