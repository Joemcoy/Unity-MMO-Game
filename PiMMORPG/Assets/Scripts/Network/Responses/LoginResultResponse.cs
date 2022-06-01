using System;
using System.IO;
using System.Linq;
using System.Text;

using PiMMORPG;
using PiMMORPG.Enums;
using PiMMORPG.Models;
using PiMMORPG.Client.Auth;

using tFramework.Helper;
using tFramework.Data.Manager;
using tFramework.Network.Interfaces;

using UnityEngine;

namespace Scripts.Network.Responses
{
    using Local;

    public class LoginResultResponse : PiAuthResponse
    {
        public override ushort ID
        {
            get { return PacketID.Login; }
        }

        LoginResult Result;
        Channel[] channels;
        Account user;
        string checksumMD5;

        public override bool Read(IDataPacket packet)
        {
            Result = packet.ReadEnum<LoginResult>();
            if(Result == LoginResult.Successful)
            {
                user = packet.ReadWrapper<Account>();
                channels = packet.ReadWrappers<Channel>();
                checksumMD5 = packet.ReadString();
            }
            return true;
        }

        public override void Execute()
        {
            var Login = GameObject.FindObjectOfType<LoginView>();
            string Message = "Unknown";

            switch(Result)
            {
                case LoginResult.Successful:
                    var cpath = Path.Combine(Environment.CurrentDirectory, "Checksum.xml");

                    if (File.Exists(cpath))
                    {
                        var hash = HashHelper.CalculateFileMD5(cpath);

                        if (hash != checksumMD5)
                            Message = "O arquivo de verificação não confere com o do servidor!";
                        else if (channels.Length == 0)
                            Message = "Nenhum servidor foi carregado!";
                        else
                        {
                            Client.User = user;

                            if (Application.Configuration.Network.LastUsername != user.Username)
                            {
                                Application.Configuration.Network.LastUsername = user.Username;
                                ConfigurationManager.Save(Application.Configuration);
                            }

                            var Menu = GameObject.FindObjectOfType<MainMenu>();
                            Menu.SwitchToChannels();

                            var View = GameObject.FindObjectOfType<ChannelsView>();
                            View.UpdateList(channels);
                        }
                    }
                    else
                        Message = "O arquivo de verificação não foi salvo ou não existe!";
                    break;
                case LoginResult.InvalidUsername:
                    Message = "Usuário não encontrado!";
                    break;
                case LoginResult.InvalidPassword:
                    Message = "Senha inválida!";
                    break;
                case LoginResult.AlreadyLogged:
                    Message = "Esta conta já se encontra logada!";
                    break;
                case LoginResult.InvalidVersion:
                    Message = "A versão do cliente não condiz com a versão do servidor!";
                    break;
                case LoginResult.Banned:
                    Message = "Essa conta foi banida!";
                    break;
            }
            Login.Status.Value = Message;
        }
    }
}