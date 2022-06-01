using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Network.Data;
using Network.Data.Interfaces;
using Game.Data;
using Game.Client;
using Data.Client;
using Base.Factories;

using Game.Data.Information;
using Game.Data.Enums;
using Game.Server.Manager;
using Game.Data.Models;
using Game.Server.Writers;

namespace Game.Server.Responses
{
    public class MovePlayerPacket : GCResponse
    {
        byte Event;
        float H, V;
        PositionModel Position;
        bool Running, Jump;

        public override uint ID { get { return PacketID.MovePlayer; } }
        public override bool Read(ISocketPacket Packet)
        {
            if(Client != null && Client.CurrentCharacter != null)
            {
                Event = Packet.ReadByte();
                LoggerFactory.GetLogger(this).LogWarning("Player {0} event: {1}", Client.CurrentCharacter.Name, Event);

                switch (Event)
                {
                    case 0:
                        LoggerFactory.GetLogger(this).LogWarning("Start event!");
                        break;
                    case 1:
                        H = Packet.ReadFloat();
                        V = Packet.ReadFloat();
                        LoggerFactory.GetLogger(this).LogWarning("H:{0} V:{1}", H, V);
                        break;
                    case 2:
                        Position = new PositionModel();
                        Position.ReadPacket(Packet);
                        Position.MapID = Client.CurrentMap.ID;
                        LoggerFactory.GetLogger(this).LogWarning("End position: {0}", Position);
                        break;
                    case 3:
                        Running = Packet.ReadBool();
                        LoggerFactory.GetLogger(this).LogWarning("Running: {0}", Running);
                        break;
                    case 4:
                        Jump = Packet.ReadBool();
                        LoggerFactory.GetLogger(this).LogWarning("Running: {0}", Jump);
                        break;
                }

                return true;
            }
            return false;
        }

        public override void Execute(IClientSocket Socket)
        {
            if(Event == 2)
                Client.CurrentCharacter.Position = Position;

            PlayerMovementWriter Packet = new PlayerMovementWriter();
            Packet.CID = Client.CurrentCharacter.ID;
            Packet.Event = Event;
            Packet.Horizontal = H;
            Packet.Vertical = V;
            Packet.Position = Position;
            Packet.Running = Running;
            Packet.Jump = Jump;

            foreach (GameClient RemoteClient in WorldManager.GetPlayersInMap(Client.CurrentMap.ID))
            {
                RemoteClient.Socket.Send(Packet);
            }
        }
    }
}
