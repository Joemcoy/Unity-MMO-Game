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
    public class AddDropPacket : DCResponse
    {
        public override uint ID { get { return PacketID.DataAddDrop; } }

        DropModel Drop;
        public override bool Read(ISocketPacket Packet)
        {
            Drop = new DropModel();
            Drop.ReadPacket(Packet);

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Base = ControllerFactory.GetBaseController("drops");
            Base.AddModel(Drop);
        }
    }
}
