using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;

using Base.Factory;
using Base.Data.Interfaces;
using Base.Data.Abstracts;

using Game.Data.Interfaces;
using Game.Data.Models;
using Game.Client;

using Server.Data.Interfaces;

using Socket.Data.Interfaces;
using Socket.Data.Enums;
using Data.Client;
using Gate.Client.Packets.Writers;
using Game.Manager.Writers;

namespace Game.Manager
{
    public class WorldManager : AWatcher, IComponent, IUpdater, ISingleton
    {
        private static readonly object syncLock = new object();
        private Dictionary<int, IPEndPoint> GateCharacters;

        public void Initalize(params object[] Params)
        {
            GateCharacters = new Dictionary<int, IPEndPoint>();
            WatcherFactory.RegisterWatcher(this);
        }

        public void Destroy()
        {
            
        }

        public override void ClientDisconnected(IClientSocket Socket, DisconnectReason Reason)
        {
            lock (syncLock)
            {
                IGameServer Server = SingletonFactory.GetSingleton<IGameServer>();
                IGameClient Client = Server.Clients.Where(C => C.Socket.EndPoint == Socket.EndPoint).FirstOrDefault();

                if (Client != null && Client.CurrentMap != null)
                {
                    DataClient Data = SingletonFactory.GetSingleton<DataClient>();
                    Data.SendUpdateCharacterPosition(Client.CurrentCharacter);

                    foreach (GameClient RemoteClient in Server.Clients.Where(C => C.CurrentMap?.ID == Client.CurrentMap.ID))
                    {
                        RemovePlayerWriter RPacket = new RemovePlayerWriter();
                        RPacket.Character = Client.CurrentCharacter;
                        RemoteClient.Socket.Send(RPacket);
                    }

                    GateCharacters.Remove(Client.CurrentCharacter.ID);
                    LoggerFactory.GetLogger().LogInfo($"Client <{Socket.EndPoint}> has been removed from world manager!");

                    SendServerMessageWriter Packet = new SendServerMessageWriter();
                    Packet.Message = $"O jogador {Client.CurrentCharacter.Name} saiu do servidor!";

                    IChatGateClient Chat = SingletonFactory.GetSingleton<IChatGateClient>();
                    Chat.Socket.Send(Packet);
                }
            }
        }

        public static bool NotifyCharacter(IGameClient Client, CharacterModel Character, MapModel Map)
        {
            lock (syncLock)
            {
                IGameServer Server = SingletonFactory.GetSingleton<IGameServer>();
                ILogger Logger = LoggerFactory.GetLogger<WorldManager>();

                SendServerMessageWriter Packet = new SendServerMessageWriter();
                WorldManager Manager = SingletonFactory.GetSingleton<WorldManager>();

                if (Manager.GateCharacters.ContainsKey(Character.ID))
                {
                    Client.Socket.Send(new AlreadyOnlineWriter());
                    return false;
                }
                else
                {
                    Packet.Message = $"O jogador {Client.CurrentCharacter.Name} acabou de entrar!";
                }

                foreach (IGameClient RemoteClient in Server.Clients)
                {
                    if (RemoteClient.CurrentMap != null && RemoteClient.CurrentMap.ID == Client.CurrentMap.ID && RemoteClient.CurrentCharacter.ID != Client.CurrentCharacter.ID)
                    {
                        SpawnPlayerWriter SPacket = new SpawnPlayerWriter();
                        SPacket.Character = Character;

                        RemoteClient.Socket.Send(SPacket);
                        Logger.LogInfo($"Spawning player {0} on map at player {1} as!");
                    }
                    else if(RemoteClient.CurrentCharacter != null && RemoteClient.CurrentMap.ID != Client.CurrentCharacter.ID)
                    {
                        RemovePlayerWriter RPacket = new RemovePlayerWriter();
                        RPacket.Character = Character;

                        RemoteClient.Socket.Send(RPacket);
                        Logger.LogInfo($"Removing player {Character.Name} on map at player {RemoteClient.CurrentCharacter.Name} as!");
                    }
                }

                Manager.GateCharacters.Add(Character.ID, Client.Socket.EndPoint);
                Logger.LogInfo($"Client <{Character.Name}> has teleported to map {Map.Name}!");

                IChatGateClient Chat = SingletonFactory.GetSingleton<IChatGateClient>();
                Chat.Socket.Send(Packet);

                Packet.Message = $"O jogador {Character.Name} foi para o mapa {Map.Name}!";
                Chat.Socket.Send(Packet);
                return true;
            }
        }

        public static IGameClient[] GetPlayersInMap(int MapID)
        {
            lock (syncLock)
            {
                IGameServer Server = SingletonFactory.GetSingleton<IGameServer>();
                return Server.Clients.Where(C => C.CurrentMap != null && C.CurrentMap.ID == MapID).ToArray();
            }
        }

        public bool Enable()
        {
            UpdaterFactory.Start(this);
            return true;
        }

        public bool Disable()
        {
            UpdaterFactory.Stop(this);
            return true;
        }

        int SaveTicks = 0;
        public void Update()
        {
            lock (syncLock)
            {
                IGameServer Server = SingletonFactory.GetSingleton<IGameServer>();
                Server.ServerTime = Server.ServerTime.AddSeconds(1);

                DataClient Data = SingletonFactory.GetSingleton<DataClient>();
                foreach (IGameClient Client in Server.Clients.Where(C => C.CurrentMap != null))
                {
                    UpdateTimeWriter Packet = new UpdateTimeWriter();
                    Packet.Time = Server.ServerTime;

                    Client.Socket.Send(Packet);

                    if (SaveTicks >= 30)
                    {
                        Data.SendUpdateCharacterPosition(Client.CurrentCharacter);
                        SaveTicks = 0;
                    }
                }

                SaveTicks++;
                Console.Title = $"Server Time = {Server.ServerTime}";
            }
        }

        public void End()
        {
            LoggerFactory.GetLogger().LogWarning($"Time updater has been stopped!");
        }

        public int Interval
        {
            get { return 1000; }
        }
    }
}