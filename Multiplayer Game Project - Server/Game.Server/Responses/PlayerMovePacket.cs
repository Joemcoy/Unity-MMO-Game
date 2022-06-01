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
    public class PlayerMovePacket : GCResponse
    {
        MoveDirection Direction;
        bool Pressed;
        PositionModel Position;

        public override uint ID { get { return PacketID.SetKeyState; } }
        public override bool Read(ISocketPacket Packet)
        {
            if (Client != null && Client.CurrentCharacter != null)
            {
                Direction = Packet.ReadEnum<MoveDirection>();
                Pressed = Packet.ReadBool();
                Position = new PositionModel();
                Position.ReadPacket(Packet);

                return true;
            }
            return false;
        }

        public override void Execute(IClientSocket Socket)
        {
            Client.CurrentCharacter.Position = Position;
            /*if (Running && Client.CurrentCharacter.Stats.Stamina > 0)
                Client.CurrentCharacter.Stats.Stamina--;
            else if (Client.CurrentCharacter.Stats.Stamina < 100)
                Client.CurrentCharacter.Stats.Stamina++;*/

            foreach (GameClient RemoteClient in WorldManager.GetPlayersInMap(Client.CurrentMap.ID))
            {
                PlayerMoveWriter Packet = new PlayerMoveWriter();
                Packet.CID = Client.CurrentCharacter.ID;
                Packet.Direction = Direction;
                Packet.Pressed = Pressed;
                Packet.Position = Position;

                RemoteClient.Socket.Send(Packet);
            }
        }
    }
}
