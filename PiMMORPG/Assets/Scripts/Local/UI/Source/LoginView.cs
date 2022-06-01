using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MarkLight;

using PiMMORPG.Client.Auth;
using tFramework.Helper;
using tFramework.Factories;

using Scripts.Network.Requests;
using Scripts.Local;
using Scripts.Local.Configuration;

public class LoginView : View
{
    public _string Username, Password, Status;

    public override void Activate()
    {
        base.Activate();
        SetDefaultValues();
        Username.Value = Application.Configuration.Network.LastUsername;
    }

    public void SwitchToMenu()
    {
        var Main = FindObjectOfType<MainMenu>();
        Main.SwitchToMenu();
    }

    public void CallLogin()
    {
        var Username = this.Username.Value;
        var Password = this.Password.Value;

        string Message = "";
        if (string.IsNullOrEmpty(Username) || Username.Replace(" ", "").Length == 0)
            Message = "Você deixou o nome de usuário em branco!";
        else if (string.IsNullOrEmpty(Password) || Password.Replace(" ", "").Length == 0)
            Message = "Você deixou a senha em branco!";
        else
        {
            Message = "Tentando conexão...";
            var client = SingletonFactory.GetSingleton<PiAuthClient>();

            if (Application.auth != null)
                Application.auth.Socket.Disconnect();
            Application.auth = client;

            if (!client.Socket.Resolve(Application.Configuration.Network.ServerAddress))
                Message = "O endereço do servidor não é válido!";
            else if (!client.Socket.Connect())
                Message = "Falha ao conectar!";
            else
            {
                var Packet = new LoginRequest();
                Packet.Username = Username;
                Packet.Password = Password.CalculateMD5();
                client.Socket.Send(Packet);

                Message = "Autenticando...";
            }
        }
        Status.Value = Message;
    }
}