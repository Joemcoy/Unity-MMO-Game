using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base.Factories;
using Data.Client;
using Game.Data;
using Game.Data.Models;
using Network.Data.Interfaces;

namespace Data.Server.Responses
{
    public class RemoveDropPacket : DCResponse
    {
        public override uint ID { get { return PacketID.DataRemoveDrop; } }

        Guid Serial;
        public override bool Read(ISocketPacket Packet)
        {
            Serial = Packet.ReadGuid();

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            var DropBase = ControllerFactory.GetBaseController("drops");
            var PositionBase = ControllerFactory.GetBaseController("drop_position");

            var Drop = DropBase.GetModels<DropModel>(M => M.Serial == Serial).First();
            DropBase.RemoveModel(Drop);
            PositionBase.RemoveModel(Drop.Position);
        }
    }
}
