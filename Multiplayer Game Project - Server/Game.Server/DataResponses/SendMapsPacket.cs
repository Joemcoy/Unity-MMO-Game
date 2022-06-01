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
    public class SendMapsPacket : DCResponse
    {
        MapModel[] Maps;

        public override uint ID { get { return PacketID.DataSendMaps; } }
        public override bool Read(ISocketPacket Packet)
        {
            Maps = new MapModel[Packet.ReadInt()];
            for (int i = 0; i < Maps.Length; i++)
            {
                Maps[i] = new MapModel();
                Maps[i].ReadPacket(Packet);
            }

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            try
            {
                WorldManager.LoadMaps(Maps);

                var Data = SingletonFactory.GetInstance<DataClient>();
                Data.SendWorldItems();
                Data.SendDrops();
            }
            catch (Exception ex)
            {
                LoggerFactory.GetLogger(this).LogFatal(ex);
                SingletonFactory.Destroy<GameServer>();
            }
        }
    }
}