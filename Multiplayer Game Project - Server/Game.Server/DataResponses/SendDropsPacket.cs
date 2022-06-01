using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Network.Data;
using Network.Data.Interfaces;
using Game.Data;
using Game.Data.Models;
using Game.Client;
using Data.Client;
using Game.Server.Manager;
using Base.Factories;
using Game.Server.Writers;

namespace Game.Server.DataResponses
{
    public class SendDropsPacket : DCResponse
    {
        DropModel Drop;
        bool End;

        public override uint ID { get { return PacketID.DataSendDrops; } }
        public override bool Read(ISocketPacket Packet)
        {
            End = Packet.ReadBool();

            if (!End)
            {
                Drop = new DropModel();
                Drop.ReadPacket(Packet);
            }

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            try
            {
                if (End)
                {
                    var Server = SingletonFactory.GetInstance<GameServer>().Socket;
                    Client.SendMobs(Server.EndPoint.Port);                    
                }
                else
                    WorldManager.AddDrop(Drop);
            }
            catch (Exception ex)
            {
                LoggerFactory.GetLogger(this).LogFatal(ex);
                SingletonFactory.Destroy<GameServer>();
            }
        }
    }
}