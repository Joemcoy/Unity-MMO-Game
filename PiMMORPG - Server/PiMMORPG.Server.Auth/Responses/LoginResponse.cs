using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Factories;
using tFramework.Network.Interfaces;

namespace PiMMORPG.Server.Auth.Responses
{
    using Enums;
    using Models;
    using Requests;

    using General.Drivers;
    using Client.Auth;

    public class LoginResponse : PiAuthResponse
    {
        public override ushort ID => PacketID.Login;

        string version, username, password;
        public override bool Read(IDataPacket packet)
        {
            version = packet.ReadString();
            username = packet.ReadString();
            password = packet.ReadString();
            return true;
        }

        public override void Execute()
        {
            var packet = new LoginRequest();
            if (version != PiConstants.Version)
                packet.Result = LoginResult.InvalidVersion;
            else
            {
                var server = SingletonFactory.GetSingleton<PiAuthServer>();
                var logger = LoggerFactory.GetLogger(this);
                /*if (server.Clients.Any(c => c.User != null && c.User.Username == username))
                    packet.Result = LoginResult.AlreadyLogged;
                else*/
                {
                    using (var ctx = new AccountDriver())
                    {
                        Account user;
                        if (ctx.HasModel(out user, ctx.CreateBuilder().Where(u => u.Username).Equal(username)))
                        {
                            if (user.Password != password)
                            {
                                packet.Result = LoginResult.InvalidPassword;
                                logger.LogWarning("Client {0} sends a login request with username {1}, but sends a invalid password!", Socket.EndPoint, username);
                            }
                            else if (user.IsBanned)
                            {
                                packet.Result = LoginResult.Banned;
                                logger.LogWarning("Client {0} sends a login request with username {1}, but this account has banned!", Socket.EndPoint, username);
                            }
                            else
                            {
                                Client.User = user;
                                packet.User = user.Clone<Account>();
                                packet.Result = LoginResult.Successful;

                                logger.LogSuccess("Client {0} sends a login request with username {1}, login successful!", Socket.EndPoint, username);
                            }
                        }
                        else
                        {
                            logger.LogWarning("Client {0} sends a login request with username {1}, that cannot be found!", Socket.EndPoint, username);
                            packet.Result = LoginResult.InvalidUsername;
                        }
                    }
                }
                Client.Socket.Send(packet);
            }
        }
    }
}