using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;

using Base.Factories;
using Base.Data.Interfaces;
using Base.Data.Abstracts;

using Game.Data.Models;
using Game.Client;



using Network.Data.Interfaces;
using Network.Data.Enums;
using Network.Data.Dispatchers;

using Data.Client;
using Gate.Client.Responses.Writers;
using Game.Server.Writers;
using Game.Data.Enums;
using Base.Helpers;
using System.IO;

namespace Game.Server.Manager
{
    public class WorldManager : ASingleton<WorldManager>, IComponent, IUpdater
    {
        private static readonly object syncLock = new object();

        private List<string> Online;
        private Dictionary<int, MapModel> Maps;
        private Dictionary<int, List<DropModel>> DropDict, CachedDrops, RemovedDrops;

        private List<MobModel> Mobs;
        private Dictionary<int, List<MobPositionModel>> MobSpawns;

        private List<TreeModel> Trees;
        private Dictionary<int, List<TreePositionModel>> TreeSpawns;

        private List<NPCModel> NPCs;
        private Dictionary<int, List<NPCPositionModel>> NPCSpawns;
        private Dictionary<int, string> Dialogues;

        private GameServer Server;
        public static bool UpdateTime { get; set; }

        protected override void Created()
        {
            UpdateTime = true;

            Online = new List<string>();
            Maps = new Dictionary<int, MapModel>();
            DropDict = new Dictionary<int, List<DropModel>>();
            CachedDrops = new Dictionary<int, List<DropModel>>();
            RemovedDrops = new Dictionary<int, List<DropModel>>();
            Dialogues = new Dictionary<int, string>();

            Server = SingletonFactory.GetInstance<GameServer>();
            Server.ClientDisconneted += (s, e) => RemovePlayer(e.Client);
        }

        public static void LoadMaps(MapModel[] Maps)
        {
            Instance.Maps = Maps.ToDictionary(M => M.ID);
        }

        public static void LoadDrops(DropModel[] Drops)
        {
            Instance.DropDict = Drops.GroupBy(M => M.Position.MapID).Select(M => M.ToList()).ToDictionary(M => M.First().ID);
        }

        public static bool NotifyCharacter(GameClient Client, CharacterModel Character)
        {
            lock (syncLock)
            {
                var Map = Instance.Maps[Character.Position.MapID];

                if (Instance.Online.Contains(Character.Name))
                {
                    Client.Socket.Send(new AlreadyOnlineWriter());
                    return false;
                }
                else
                {
                    Client.Socket.Disconnected += (s, e) => RemovePlayer(Client);

                    Instance.Online.Add(Character.Name);
                    AddPlayer(Client);

                    return true;
                }
            }
        }

        public static void AddPlayer(GameClient Client, int LastMap = -1)
        {
            Client.Socket.Send(new UpdateTimeWriter { Time = Instance.Server.ServerTime });
            ILogger Logger = LoggerFactory.GetLogger<WorldManager>();

            var Message = new MessageModel();
            Message.Type = MessageType.System;
            Message.Access = AccessLevel.Server;

            foreach (var Remote in Instance.Server.OnlineClients.Where(C => C.CurrentCharacter.ID != Client.CurrentCharacter.ID))
                if (LastMap != -1 && Remote.CurrentMap.ID == LastMap)
                {
                    Remote.Socket.Send(new RemovePlayerWriter { Character = Client.CurrentCharacter });

                    Message.Content = "LM:Messages.Player Exit";
                    ChatManager.SendToClient(Remote, Message, Client.CurrentCharacter.Name);
                }
                else if (Remote.CurrentMap.ID == Client.CurrentMap.ID)
                    Remote.Socket.Send(new SpawnPlayerWriter { Character = Client.CurrentCharacter });

            /*List<int> TempMaps = new List<int>();
            foreach (GameClient RemoteClient in Server.Clients.Where(C => C.CurrentCharacter != null))
            {
                if (RemoteClient.CurrentMap.ID == Client.CurrentMap.ID && RemoteClient.CurrentCharacter.ID != Client.CurrentCharacter.ID)
                {
                    SpawnPlayerWriter SPacket = new SpawnPlayerWriter();
                    SPacket.Character = Client.CurrentCharacter;

                    RemoteClient.Socket.Send(SPacket);
                    Logger.LogInfo("Spawning player {0} on map at player {1} as!", Client.CurrentCharacter.Name, Client.CurrentMap.Name);
                }
                else if (RemoteClient.CurrentMap.ID != Client.CurrentMap.ID)
                {
                    RemovePlayerWriter RPacket = new RemovePlayerWriter();
                    RPacket.Character = Client.CurrentCharacter;

                    RemoteClient.Socket.Send(RPacket);
                    Logger.LogInfo("Removing player {0} on map at player {1} as!", Client.CurrentCharacter.Name, RemoteClient.CurrentCharacter.Name);

                    if (!TempMaps.Contains(RemoteClient.CurrentMap.ID))
                    {
                        Message.Content = "LM:Messages.Player Exit";
                        ChatManager.SendToAllInMap(RemoteClient.CurrentMap.ID, Message, Client.CurrentCharacter.Name);

                        TempMaps.Add(RemoteClient.CurrentMap.ID);
                    }
                }
            }*/
            Logger.LogInfo("Client <{0}> has teleported to map {1}!", Client.CurrentCharacter.Name, Client.CurrentMap.Name);

            Message.Content = "LM:Messages.Player Enter";
            ChatManager.SendToAllInMap(Client.CurrentCharacter.Position.MapID, Message, Client.CurrentCharacter.Name);
        }

        public static void RemovePlayer(GameClient Client)
        {
            lock (syncLock)
            {
                if (Client.CurrentMap != null)
                {
                    DataClient Data = SingletonFactory.GetInstance<DataClient>();
                    Data.SendUpdateCharacterPosition(Client.CurrentCharacter);

                    var ItemManager = SingletonFactory.GetInstance<ItemCacheManager>();
                    ItemManager.SaveClient(Client);

                    foreach (GameClient RemoteClient in Instance.Server.OnlineClients.Where(C => C.CurrentMap.ID == Client.CurrentMap.ID))
                    {
                        RemovePlayerWriter RPacket = new RemovePlayerWriter();
                        RPacket.Character = Client.CurrentCharacter;
                        RemoteClient.Socket.Send(RPacket);
                    }

                    Instance.Online.Remove(Client.CurrentCharacter.Name);
                    LoggerFactory.GetLogger(Instance).LogInfo("Client <{0}> has been removed from world manager!", Client.Socket.EndPoint);

                    var Message = new MessageModel();
                    Message.Content = "LM:Messages.Player Exit";
                    Message.Type = MessageType.System;
                    Message.Access = AccessLevel.Server;

                    ChatManager.SendToAllInMap(Client.CurrentMap.ID, Message, Client.CurrentCharacter.Name);
                }
                else
                    LoggerFactory.GetLogger(Instance).LogWarning("Failed to remove character {0}!", Client.Socket.EndPoint);
            }
        }

        public static void TeleportPlayer(GameClient Client, MapModel Map)
        {
            RemovePlayer(Client);
            var Last = Client.CurrentMap.ID;

            Client.CurrentMap = Map;
            Client.CurrentCharacter.Position = Map.Spawn;
            AddPlayer(Client, Last);

            var Packet = new PlayerListWriter();
            var Cache = SingletonFactory.GetInstance<ItemCacheManager>();

            Packet.Map = Map;
            Packet.CID = Client.CurrentCharacter.ID;
            Packet.Items = Cache.GetWorldItems(Map.ID);
            Client.Socket.Send(Packet);
        }

        public static MapModel GetMapByID(int ID)
        {
            lock (syncLock)
            {
                return Instance.Maps[ID];
            }
        }

        public static GameClient[] GetPlayersInMap(int MapID)
        {
            lock (syncLock)
            {
                GameServer Server = SingletonFactory.GetInstance<GameServer>();
                return Server.Clients.Where(C => C.CurrentMap != null && C.CurrentMap.ID == MapID).ToArray();
            }
        }

        public static void AddDrop(DropModel Drop)
        {
            lock (syncLock)
            {
                if (!Instance.DropDict.ContainsKey(Drop.Position.MapID))
                    Instance.DropDict.Add(Drop.Position.MapID, new List<DropModel>());
                Instance.DropDict[Drop.Position.MapID].Add(Drop);
            }
        }

        public static DropModel RemoveDrop(int MapID, Guid Serial)
        {
            lock (syncLock)
            {
                DropModel Drop = null;
                bool Flag = true;

                if (Instance.DropDict.ContainsKey(MapID))
                {
                    Flag = false;
                    Drop = Instance.DropDict[MapID].FirstOrDefault(M => M.Serial == Serial);
                }

                if (!Flag && Drop == null || Flag)
                {
                    Flag = true;
                    Drop = Instance.CachedDrops[MapID].FirstOrDefault(M => M.Serial == Serial);
                }

                if (Flag)
                    Instance.CachedDrops[MapID].Remove(Drop);
                else
                    Instance.DropDict[MapID].Remove(Drop);

                if (!Instance.RemovedDrops.ContainsKey(MapID))
                    Instance.RemovedDrops.Add(MapID, new List<DropModel>());
                Instance.RemovedDrops[MapID].Add(Drop);

                return Drop;
            }
        }

        DropModel[] GetAllDrops(int MapID)
        {
            if (DropDict.ContainsKey(MapID))
            {
                var Drops = this.DropDict[MapID].ToArray();
                if (CachedDrops.ContainsKey(MapID))
                    Drops = Drops.Concat(CachedDrops[MapID]).ToArray();

                return Drops;
            }
            else if (CachedDrops.ContainsKey(MapID))
            {
                return CachedDrops[MapID].ToArray();
            }
            else
                return new DropModel[0];
        }

        public static DropModel[] GetDrops(int MapID)
        {
            lock (syncLock)
            {
                var Drops = Instance.GetAllDrops(MapID).ToArray();

                return Drops;
            }
        }

        public static void RegisterMobs(MobModel[] Mobs, Dictionary<int, List<MobPositionModel>> Spawns)
        {
            Instance.Mobs = new List<MobModel>(Mobs);
            Instance.MobSpawns = Spawns;

            LoggerFactory.GetLogger<WorldManager>().LogInfo("Cached {0} mobs and {1} spawns!", Mobs.Length, Spawns.SelectMany(S => S.Value).Count());
        }

        public static MobModel[] GetMobs() { return Instance.Mobs.ToArray(); }
        public static MobPositionModel[] GetMobSpawns(int MapID) { return Instance.MobSpawns.SelectMany(K => K.Value.Where(M => M.MapID == MapID)).ToArray(); }
        public static MobModel GetMobByID(int ID) { return Instance.Mobs.FirstOrDefault(M => M.ID == ID); }

        public static void RegisterTrees(TreeModel[] Trees, Dictionary<int, List<TreePositionModel>> Spawns)
        {
            Instance.Trees = new List<TreeModel>(Trees);
            Instance.TreeSpawns = Spawns;

            LoggerFactory.GetLogger<WorldManager>().LogInfo("Cached {0} tree and {1} spawns!", Trees.Length, Spawns.SelectMany(S => S.Value).Count());
        }

        public static TreeModel[] GetTrees() { return Instance.Trees.ToArray(); }
        public static TreePositionModel[] GetTreeSpawns(int MapID) { return Instance.TreeSpawns.SelectMany(K => K.Value.Where(M => M.MapID == MapID)).ToArray(); }
        public static TreeModel GetTreeByID(int ID) { return Instance.Trees.FirstOrDefault(M => M.ID == ID); }

        public static void RegisterNPCs(NPCModel[] NPCs, Dictionary<int, List<NPCPositionModel>> Spawns)
        {
            Instance.NPCs = new List<NPCModel>(NPCs);
            Instance.NPCSpawns = Spawns;

            foreach (var Spawn in Spawns.Values.SelectMany(I => I.ToArray()))
            {
                if (Spawn.HasDialogue)
                {
                    var Filename = Path.Combine(Environment.CurrentDirectory, "dialogues", string.Format("{0}.jsd", Spawn.ID));
                    var Buffer = RijndaelHelper.Decrypt(File.ReadAllBytes(Filename));

                    Instance.Dialogues[Spawn.ID] = Encoding.UTF8.GetString(Buffer);
                    LoggerFactory.GetLogger<WorldManager>().LogSuccess("Dialogue of NPC Spawn {0} has been loaded!", Spawn.ID);
                }
            }

            LoggerFactory.GetLogger<WorldManager>().LogInfo("Cached {0} npcs and {1} spawns!", NPCs.Length, Spawns.SelectMany(S => S.Value).Count());
        }

        public static NPCModel[] GetNPCs() { return Instance.NPCs.ToArray(); }
        public static NPCPositionModel[] GetNPCSpawns(int MapID) { return Instance.NPCSpawns.SelectMany(K => K.Value.Where(M => M.MapID == MapID)).ToArray(); }
        public static NPCModel GetNPCByID(int ID) { return Instance.NPCs.FirstOrDefault(M => M.ID == ID); }
        public static string GetNPCDialogue(int ID) { return Instance.Dialogues[ID]; }

        bool IComponent.Enable()
        {
            UpdaterFactory.Start(this);
            return true;
        }

        bool IComponent.Disable()
        {
            UpdaterFactory.Stop(this);
            return true;
        }

        void IUpdater.Start()
        {

        }

        int SaveTicks = 0;
        void IUpdater.Loop()
        {
            lock (syncLock)
            {
                GameServer Server = SingletonFactory.GetInstance<GameServer>();

                if (UpdateTime)
                    Server.ServerTime = Server.ServerTime.AddSeconds(36);

                UpdateTimeWriter Packet = new UpdateTimeWriter();
                Packet.Time = Server.ServerTime;

                DataClient Data = SingletonFactory.GetInstance<DataClient>();
                foreach (GameClient Client in Server.Clients.Where(C => C.CurrentMap != null))
                {
                    if (UpdateTime)
                        Client.Socket.Send(Packet);

                    if (SaveTicks >= 30)
                    {
                        Data.SendUpdateCharacterPosition(Client.CurrentCharacter);
                        SaveTicks = 0;
                    }
                }

                foreach (var Drop in DropDict.Values.SelectMany(M => M))
                {
                    Data.SendAddDrop(Drop);

                    if (!CachedDrops.ContainsKey(Drop.Position.MapID))
                        CachedDrops[Drop.Position.MapID] = new List<DropModel>();
                    CachedDrops[Drop.Position.MapID].Add(Drop);
                }

                foreach (var Drop in RemovedDrops.Values.SelectMany(M => M))
                {
                    Data.RemoveDrop(Drop.Serial);
                }

                DropDict.Clear();
                RemovedDrops.Clear();

                SaveTicks++;
                Console.Title = string.Format("Server Time = {0}", Server.ServerTime);
            }
        }

        void IUpdater.End()
        {
            LoggerFactory.GetLogger(this).LogWarning("Time updater has been stopped!");
        }

        int IUpdater.Interval
        {
            get { return 1000; }
        }
    }
}