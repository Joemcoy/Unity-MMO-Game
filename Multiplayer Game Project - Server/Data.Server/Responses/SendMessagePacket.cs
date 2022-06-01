using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Network.Data;using Network.Data.Interfaces;
using Game.Data;
using Game.Data.Results;
using Game.Data.Models;
using Game.Controller;
using Data.Client;
using Base.Factories;

namespace Data.Server.Responses
{
    public class SendMessagePacket : DCResponse
    {
        MessageModel Message;

        public override uint ID { get { return PacketID.DataSendMessage; } }
        public override bool Read(ISocketPacket Packet)
        {
            Message = new MessageModel();
            Message.ReadPacket(Packet);

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            ControllerFactory.GetBaseController("chat_log").AddModel(Message);
        }
    }
}
