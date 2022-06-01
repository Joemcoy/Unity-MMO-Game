using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data;
using Network.Data;
using Network.Data.Interfaces;

using Auth.Client;
using Data.Client;

using Game.Controller;
using Game.Data.Models;
using Game.Data.Results;
using Base.Factories;
using Auth.Server.Requests;
using Game.Data.Information;

namespace Auth.Server.Responses
{
    public class LoginPacket : ACResponse
    {
        VersionInfo Version;
        string Username, Password;

        public override uint ID { get { return PacketID.Login; } }
        public override bool Read(ISocketPacket Packet)
        {
            Version = new VersionInfo();
            Version.ReadPacket(Packet);

            Username = Packet.ReadString();
            Password = Packet.ReadString();

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            var Packet = new LoginResultRequest();

            if (!Version.Equals(GConstants.Version))
            {
                LoggerFactory.GetLogger(this).LogWarning("Client {0} has send invalid version! ({1} != {2})", Username, Version, GConstants.Version);

                Packet.Result = LoginResult.InvalidVersion;
                Socket.Send(Packet);
            }
            else
            {
                LoggerFactory.GetLogger(this).LogWarning($"Client {Socket.EndPoint} wait for data!");
                var Server = Convert.ToUInt32(Socket.Server.EndPoint.Port);

                DataClient Client = SingletonFactory.GetInstance<DataClient>();
                Client.SendLoginRequest(this.Client, Username, Password, Server);
            }
        }
    }
}