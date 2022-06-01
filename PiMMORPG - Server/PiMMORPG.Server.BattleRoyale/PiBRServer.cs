using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Network.Enums;
namespace PiMMORPG.Server.BattleRoyale
{
    using Manager;

    using General;
    using General.Bases;
    using General.Drivers;

    using Client;
    using Client.BattleRoyale;

    public class PiBRServer : GameServerBase<PiBRServer, PiBRClient>
    {        
        public PiBRServer()
        {
            RegisterResponses(typeof(ServerControl).Assembly);
            RegisterResponses();
            RegisterResponses<PiBRResponse>();
        }

        protected override void Connected(PiBaseClient client)
        {
            base.Connected(client);
        }

        protected override void Disconnected(PiBRClient client, DisconnectReason reason)
        {
            if(client.RoomID != Guid.Empty)
            {
                var room = RoomManager.GetRoomByID(client.RoomID);
                if (room != null)
                    room.RemoveClient(client);
                else
                    logger.LogWarning("Client roomID out of sync!");
            }
        }
    }
}