using Data.Client;
using Game.Controller;
using Game.Data;
using Game.Data.Models;
using Network.Data;
using Network.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Base.Factories;

namespace Data.Server.Responses
{
    public class RemoveItemPacket : DCResponse
    {
        Guid Serial;

        public override uint ID { get { return PacketID.DataRemoveItem; } }
        public override bool Read(ISocketPacket Packet)
        {
            Serial = Packet.ReadGuid();

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            LoggerFactory.GetLogger(this).LogInfo("Removing item {0}!", Serial);
            CharacterItemManager.RemoveItemBySerial(Serial);
        }
    }
}
