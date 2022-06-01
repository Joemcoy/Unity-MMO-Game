using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

using Base.Factories;
using Base.Configurations;
using Base.Data.Interfaces;

using Server.Configuration;


using Game.Client;
using Game.Data.Enums;

using Gate.Client;
using Gate.Client.Responses.Writers;

using Network.v1;
using Network.Data.EventArgs;
using Network.Data.Interfaces;

using Data.Client;
using Game.Data.Information;
using Network.Bases;
using System.Reflection;
using Game.Server.Manager;

namespace Game.Server
{
	public class GameServer : ServerBase<GameClient>, ISingleton, IComponent
    {
        private GateClient  Gate;

        protected override Assembly ResponsesAssembly { get { return typeof(GameServer).Assembly; } }
        public event EventHandler<EventArgs.ClientDisconnectedEventArgs> ClientDisconneted;
		public DateTime ServerTime { get; set; }
        public GameClient[] OnlineClients { get { return Clients.Where(C => C.CurrentCharacter != null && C.CurrentMap != null).ToArray(); } }

        bool IComponent.Enable()
		{
            try
            {
                int Port = int.Parse(Environment.GetCommandLineArgs()[1]);
                bool PVP = int.Parse(Environment.GetCommandLineArgs()[2]) == 1;
                int MaximumClients = int.Parse(Environment.GetCommandLineArgs()[3]);
                string Address = Environment.GetCommandLineArgs()[4];
                string Name = string.Join(" ", Environment.GetCommandLineArgs().Skip(5).ToArray());

                Gate = SingletonFactory.GetInstance<GateClient>();

                Gate.Port = Port;
                Gate.Name = Name;
                Gate.PVP = PVP;
                Gate.Type = GateType.Game;
                Gate.Address = Address;
                Gate.MaximumClients = MaximumClients;

                if (!ComponentFactory.Enable<GateClient>())
                    return false;
                else
                {
                    DataClient Data = SingletonFactory.GetInstance<DataClient>();
                    if (!ComponentFactory.Enable<IntervalConfiguration>())
                        return false;
                    else if (!ComponentFactory.Enable<GameConfiguration>())
                        return false;
                    else if (!ComponentFactory.Enable<DataClient>())
                        return false;
                    else if (!ComponentFactory.Enable<Game.Server.Manager.WorldManager>())
                        return false;                    
                    else
                    {
                        Data.Socket.RegisterResponse<DCResponse>();
                        Data.Socket.IOEnabled = true;

                        SendGateTypeWriter Packet = new SendGateTypeWriter();
                        Packet.Gate = Gate.Info;
                        Packet.Type = GateType.Game;

                        Gate.Socket.Send(Packet);
                        ServerTime = DateTime.Now;
                        
                        Socket.EndPoint.Port = Port;

                        Data.SendMapsRequest();

                        LoggerFactory.GetLogger(this).LogWarning("Game server has been started, waiting for map cache..");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerFactory.GetLogger(this).LogFatal(ex);
                return false;
            }
		}

		bool IComponent.Disable()
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

		void ISingleton.Create()
		{
            Socket.ClientConnected += Socket_ClientConnected;
        }

        private void Socket_ClientConnected(object sender, ClientConnectedEventArgs e)
        {
            var Packet = new UpdatePlayerCountWriter();
            Packet.Increment = true;
            Gate.Socket.Send(Packet);

            e.Client.Disconnected += Socket_ClientDisconnected;
        }

        private void Socket_ClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            var Packet = new UpdatePlayerCountWriter();
            Packet.Increment = false;
            Gate.Socket.Send(Packet);

            GameClient Client = null;

            if (ClientDisconneted != null && (Client = Clients.FirstOrDefault(C => C.Socket.Equals(e.Client))) != null)
                ClientDisconneted(this, new EventArgs.ClientDisconnectedEventArgs(Client));
        }

        private void Server_Opened(object sender, ServerSocketEventArgs e)
        {
            LoggerFactory.GetLogger(this).LogInfo("Game server has been opened!");
        }

        private void Server_Closed(object sender, ServerSocketEventArgs e)
        {
            LoggerFactory.GetLogger(this).LogInfo("Game server has been closed!");
        }

        void ISingleton.Destroy()
		{
            
		}

        public void Listen()
        {
            if (!ComponentFactory.Enable<ItemCacheManager>())
                throw new Exception("Failed to initalize the Item cache manager!");
            else if (!ComponentFactory.Enable<CommandFactory<GCommand>>())
                throw new Exception("Failed to initalize the command factory!");
            else if (!Socket.Open())
                throw new Exception("Failed to open the game server!");
            else
            {
                var Client = SingletonFactory.GetInstance<DataClient>();
                Client.SendWorldItems();
            }
        }
    }
}
