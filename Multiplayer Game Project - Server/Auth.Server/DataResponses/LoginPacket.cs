using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Game.Data.Results;
using Network.Data;
using Network.Data.Interfaces;

using Auth.Client;
using Data.Client;
using Game.Data.Models;
using Gate.Server;
using Game.Data.Information;
using Gate.Client;
using Game.Data.Enums;
using Base.Factories;
using Auth.Server.Requests;
using System.IO;

namespace Auth.Server.DataResponses
{
    public class LoginPacket : DCResponse
    {
        LoginResult Result;
        AccountModel Account;

        public override uint ID { get { return PacketID.DataSendLogin; } }
        public override bool Read(ISocketPacket Packet)
        {
            Result = Packet.ReadEnum<LoginResult>();
            if (Result == LoginResult.Success)
            {
                Account = new AccountModel();
                Account.ReadPacket(Packet);
            }

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            AuthClient Client = this.Client.Dequeue<AuthClient>();
            var Packet = new LoginResultRequest();
            
            LoggerFactory.GetLogger(this).LogInfo("Client {0} received data!", Client.Socket.EndPoint);
            if (Result == LoginResult.Success)
            {
                var Server = SingletonFactory.GetInstance<GateServer>();
                Packet.Gates = Server.Clients.Where(C => C.Type == GateType.Game).Select(C => C.Info).ToArray();

                LoggerFactory.GetLogger(this).LogInfo("Game Gates = {0} | {1}", Packet.Gates.Length, Packet.Gates.Length == 0 ? "Empty" : string.Join(", ", Packet.Gates.Select(g => g.Name).ToArray()));
                if(Client.Server.Clients.Any(C => C.Account != null && C.Account.Username == Account.Username))
                {
                    Packet.Result = LoginResult.AlreadyLogged;

                    var LogPath = Path.Combine(Environment.CurrentDirectory, "Logs", "Double Login Log.txt");
                    if (!File.Exists(LogPath))
                        File.Create(LogPath).Close();

                    File.WriteAllText(LogPath, File.ReadAllText(LogPath) + Environment.NewLine + string.Format("{0} => {1}", Account.Username, Client.Socket.EndPoint.ToString()));
                }
                else if (Packet.Gates.Length > 0)
                {
                    Account.LastIP = Client.Socket.EndPoint.Address.ToString();
                    this.Client.SendUpdateAccount(Account);

                    Client.Account = Account;
                    Packet.Result = LoginResult.Success;
                    Packet.Account = Client.Account;
                }
                else
                    Packet.Result = LoginResult.NoGameGates;
            }
            else
                Packet.Result = Result;

            Client.Socket.Send(Packet);   
        }
    }
}