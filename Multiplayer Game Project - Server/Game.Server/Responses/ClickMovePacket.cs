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
    public class ClickMovePacket : GCResponse
    {
        float X, Y, Z;

        public override uint ID { get { return PacketID.ClickMove; } }
        public override bool Read(ISocketPacket Packet)
        {
            if (Client != null && Client.CurrentCharacter != null)
            {
                X = Packet.ReadFloat();
                Y = Packet.ReadFloat();
                Z = Packet.ReadFloat();

                return true;
            }
            return false;
        }

        public override void Execute(IClientSocket Socket)
        {
            //Client.CurrentCharacter.Position = Position;
            /*if (Running && Client.CurrentCharacter.Stats.Stamina > 0)
                Client.CurrentCharacter.Stats.Stamina--;
            else if (Client.CurrentCharacter.Stats.Stamina < 100)
                Client.CurrentCharacter.Stats.Stamina++;*/

            Client.CurrentCharacter.Position.PositionX = X;
            Client.CurrentCharacter.Position.PositionY = Y;
            Client.CurrentCharacter.Position.PositionZ = Z;

            LoggerFactory.GetLogger(this).LogWarning("Moving character {0} to position {1},{2},{3}!", Client.CurrentCharacter.ID, X, Y, Z);
            foreach (var Remote in WorldManager.GetPlayersInMap(Client.CurrentMap.ID))
            {
                LoggerFactory.GetLogger(this).LogWarning("Sending position to character {0}!", Remote.CurrentCharacter.ID);
                Send(Remote);
            }
        }

        void Send(GameClient Remote)
        {
            var Packet = new ClickMoveWriter();
            Packet.CID = Client.CurrentCharacter.ID;
            Packet.X = X;
            Packet.Y = Y;
            Packet.Z = Z;

            Remote.Socket.Send(Packet);
        }
    }
}
