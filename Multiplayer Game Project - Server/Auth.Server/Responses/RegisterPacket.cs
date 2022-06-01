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
using System.IO;
using Auth.Server.Requests;

namespace Auth.Server.Responses
{
    public class RegisterPacket : ACResponse
    {
        string Username, Password, Nickname, Email;

        public override uint ID { get { return PacketID.Register; } }
        public override bool Read(ISocketPacket Packet)
        {
            Username = Packet.ReadString();
            Password = Packet.ReadString();
            Nickname = Packet.ReadString();
            Email = Packet.ReadString();

            return true;
        }

        public override void Execute(IClientSocket Socket)
        {
            if (GConstants.Version.Name == "Alpha" && !File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "Subscribers.txt")).Any(L => L == Email))
            {
                var Packet = new RegisterResultRequest();
                Packet.Result =  RegisterResult.NonRegistered;

                Client.Socket.Send(Packet);

                var LogPath = Path.Combine(Environment.CurrentDirectory, "Logs", "Non registered emails.txt");
                if (!File.Exists(LogPath))
                    File.Create(LogPath).Close();

                File.WriteAllText(LogPath, File.ReadAllText(LogPath) + Environment.NewLine + string.Format("{0}|{1} => {2}",Username, Email, Client.Socket.EndPoint.ToString()));
            }
            else
            {
                DataClient Client = SingletonFactory.GetInstance<DataClient>();
                var Server = Convert.ToUInt32(Socket.Server.EndPoint.Port);

                Client.SendRegisterRequest(this.Client, Username, Password, Nickname, Email, Server);
            }
        }
    }
}
