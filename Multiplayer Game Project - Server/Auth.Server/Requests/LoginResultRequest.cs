using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Game.Data;
using Game.Data.Results;
using Auth.Client;
using Base.Factories;
using Network.Data.Interfaces;
using Gate.Server;
using Game.Data.Models;
using Game.Data.Information;

namespace Auth.Server.Requests
{
    public class LoginResultRequest : IRequest
    {
        public uint ID { get { return PacketID.Login; } }
        public LoginResult Result { get; set; }
        public AccountModel Account { get; set; }
        public GateInfo[] Gates { get; set; }

        public bool Write(IClientSocket Client, ISocketPacket Packet)
        {
            try
            {
                Packet.WriteEnum(Result);

                if (Result == LoginResult.Success)
                {
                    var Server = SingletonFactory.GetInstance<AuthServer>();

                    Account.WritePacket(Packet);                    
                    Packet.WriteInt(Gates.Length);
                    Gates.ForEach(g => g.WritePacket(Packet));

                    Packet.WriteInt(Server.Files.Length);
                    foreach (var File in Server.Files)
                    {
                        File.WritePacket(Packet);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                LoggerFactory.GetLogger(this).LogFatal(ex);
                return false;
            }
        }
    }
}