using System;
using System.Linq;
using System.Collections.Generic;

using tFramework.Enums;
using tFramework.Factories;
using tFramework.Interfaces;

namespace PiMMORPG.Server.BattleRoyale
{
    using Requests;
    using Manager;

    using General;
    using General.Drivers;
    using General.Requests;
    using Client.BattleRoyale;
    using Client.BattleRoyale.Enums;

    public class Room : IUpdater
    {
        ILogger logger;
        List<PiBRClient> clients;
        Queue<Action> actions;
        TimeSpan time, second = TimeSpan.FromSeconds(1), zero = TimeSpan.FromSeconds(0), waterUp = TimeSpan.FromMinutes(1);

        UpdateTimeRequest timeRequest;
        ElevateWaterRequest waterRequest;
        UpdateRoomRequest statusRequest;
        
        int lastWater = 0;

        public Guid ID { get; }
        public RoomState State { get; private set; }
        public PiBRClient[] Clients { get { return clients.ToArray(); } }
        public TimeSpan Time { get { return time; } }
        public TimeSpan Add { get; set; }
        public TimeSpan Timeout { get; set; }
        public int WaterLevel { get; private set; }
        public bool UpdateTime { get; set; }

        int IUpdater.Interval { get { return 1000; } }
        DelayMode IUpdater.DelayMode { get { return DelayMode.DelayAfter; } }

        public Room(PiBRClient owner)
        {
            ID = Guid.NewGuid();
            logger = LoggerFactory.GetLogger(this);
            time = new TimeSpan(6, 0, 0);
            Add = TimeSpan.FromSeconds(60);
            Timeout = TimeSpan.FromMinutes(1);
            State = RoomState.WaitingForPlayer;
            UpdateTime = true;

            timeRequest = new UpdateTimeRequest();
            waterRequest = new ElevateWaterRequest();
            statusRequest = new UpdateRoomRequest();

            clients = new List<PiBRClient>();
            actions = new Queue<Action>();

            owner.RoomID = ID;
            clients.Add(owner);
            ThreadFactory.Start(this);
        }

        void IThread.Start()
        {
            logger.LogInfo("Room {0} has been started (owner: {1})!", ID, clients[0].Character.Name);
        }

        bool IThread.Run()
        {
            if (clients.Count > 0)
            {
                if (actions.Count > 0)
                {
                    var act = actions.Dequeue();
                    act.Invoke();
                }
                else
                {
                    var t = timeRequest.Time = time;
                    clients.ForEach(c => c.Socket.Send(timeRequest));
                    if (UpdateTime)
                        time = time.Add(Add);

                    if (lastWater != WaterLevel)
                    {
                        lastWater = waterRequest.Level = WaterLevel;
                        clients.ForEach(c => c.Socket.Send(waterRequest));
                    }

                    if (State == RoomState.WaitingForPlayer && Timeout.Minutes != 1)
                        Timeout = TimeSpan.FromMinutes(1);
                    else if (State != RoomState.WaitingForPlayer && clients.Count <= 1)
                        return false;
                    else if (State == RoomState.WaitingTimeout || State == RoomState.Starting)
                    {
                        Timeout = Timeout.Subtract(second);
                        if (Timeout == zero)
                        {
                            if (State == RoomState.WaitingTimeout)
                            {
                                Timeout = TimeSpan.FromSeconds(10);
                                State = RoomState.Starting;
                            }
                            else if (State == RoomState.Starting)
                            {
                                var temp = new List<uint>();
                                using (var ctx = new SpawnDriver())
                                {
                                    var spawns = ctx.GetModels(ctx.CreateBuilder().Where(m => m.MapID).Equal(ServerControl.Configuration.BattleRoyaleMap));
                                    var packet = new MoveCharacterRequest();
                                    foreach (var client in clients)
                                    {
                                        var pos = spawns.Where(s => !temp.Contains(s.ID)).OrderBy(c => Guid.NewGuid()).First();
                                        packet.CharacterID = client.Character.ID;
                                        packet.Position = client.Character.Position = pos;
                                        clients.ForEach(c => c.Socket.Send(packet));
                                        temp.Add(pos.ID);

                                        logger.LogWarning("Moving player {0} to spawn {1}:{2},{3},{4}!", client.Character.Name, pos.ID, pos.PositionX, pos.PositionY, pos.PositionZ);
                                    }
                                }

                                Timeout = waterUp;
                                State = RoomState.Running;
                            }
                        }
                    }
                    else if (State == RoomState.Running)
                    {
                        Timeout = Timeout.Subtract(second);
                        if (Timeout == zero)
                        {
                            WaterLevel += 10;
                            Timeout = waterUp;
                        }
                    }

                    //if (!managing)
                    //{
                        //sending = true;
                        statusRequest.State = State;
                        statusRequest.WaterLevel = WaterLevel;
                        statusRequest.PlayerCount = clients.Count;
                        statusRequest.Timeout = Timeout;
                        clients.ForEach(c => c.Socket.Send(statusRequest));
                        //sending = false;
                    //}
                }
                return true;
            }
            return false;
        }

        void IThread.End()
        {
            clients.ForEach(c => c.RoomID = Guid.Empty);
            clients.ForEach(c => c.Socket.Disconnect());
            clients.Clear();
            logger.LogInfo("Room {0} has been ended!", ID);

            RoomManager.RemoveRoom(this);
        }

        public void AddClient(PiBRClient client)
        {
            if (State == RoomState.Running)
                throw new NotSupportedException();

            client.RoomID = ID;

            actions.Enqueue(() =>
            {
                var spawn = new SpawnCharacterRequest { Character = client.Character };
                clients.ForEach(c => c.Socket.Send(spawn));
                clients.Add(client);
                logger.LogInfo("Client {0} entered on room {1}!", client.Character.Name, ID);

                if (State == RoomState.WaitingForPlayer && clients.Count > 1)
                    State = RoomState.WaitingTimeout;
            });
        }

        public void RemoveClient(PiBRClient client)
        {
            client.RoomID = Guid.Empty;

            actions.Enqueue(() =>
            {
                var despawn = new RemoveCharacterRequest { CharacterId = client.Character.ID };
                clients.Remove(client);
                clients.ForEach(c => c.Socket.Send(despawn));
                logger.LogInfo("Client {0} has exited from room {1}!", client.Character.Name, ID);

                if (State == RoomState.WaitingTimeout && clients.Count == 1)
                    State = RoomState.WaitingForPlayer;
                else if (State == RoomState.WaitingForPlayer)
                    ThreadFactory.Stop(this);
            });
        }

        public void SetTime(int hours, int minutes)
        {
            time = new TimeSpan(hours, minutes, 0);
            /*if (!managing)
            {
                sending = true;
                clients.ForEach(c => c.Socket.Send(timeRequest));
                sending = false;
            }*/
        }

        public void SetWaterLevel(int level)
        {
            WaterLevel = level;
            logger.LogInfo("Water level of room {0} changed to {1}!", ID, WaterLevel);
        }
    }
}