using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Game.Client;
using Game.Server.Manager;
using Game.Server.Writers;

using Network.Data.Interfaces;
using Base.Factories;

namespace Game.Server.Responses
{
    public class PlayMotionPacket : GCResponse
    {
        public override uint ID { get { return PacketID.PlayMotion; } }

        string TriggerName;
        public override bool Read(ISocketPacket Packet)
        {
            TriggerName = Packet.ReadString();
            LoggerFactory.GetLogger(this).LogWarning("Received trigger {0} from client {1}!", TriggerName, Client.CurrentCharacter.ID);

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Packet = new PlayMotionWriter();
            Packet.CID = Client.CurrentCharacter.ID;
            Packet.TriggerName = TriggerName;

            foreach (var Remote in WorldManager.GetPlayersInMap(Client.CurrentMap.ID))
            {
                Remote.Socket.Send(Packet);
                LoggerFactory.GetLogger(this).LogWarning("Trigger {0} sent to client {1}!", TriggerName, Remote.CurrentCharacter.ID);
            }
        }
    }
}
