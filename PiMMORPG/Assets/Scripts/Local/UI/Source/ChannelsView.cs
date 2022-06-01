using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

using PiMMORPG.Models;
using PiMMORPG.Enums;
using PiMMORPG.Client.Interfaces;
using PiMMORPG.Client.Auth;
using PiMMORPG.Client.RPG;
using PiMMORPG.Client.BattleRoyale;

using MarkLight;
using tFramework.Factories;

using Scripts.Local;
using Scripts.Local.Control;
using Scripts.Network.Requests.GameClient;

public class ChannelsView : View
{
    public ObservableList<Channel> Channels;
    public _string Status;
    public _bool CanConnect;

    public override void Activate()
    {
        base.Activate();
        SetDefaultValues();
    }

    public override void Initialize()
    {
        base.Initialize();
        Channels = new ObservableList<Channel>();
    }

    public string IsPVP(Channel channel)
    {
        if (channel == null)
            return "Undefined";

        if (channel.Type == ServerType.RPG)
            return "PV" + (channel.IsPVP ? "P" : "E");
        else
            return string.Format("BR({0})", channel.MaximumConnections);
    }

    public string ChannelPopl(Channel channel)
    {
        if (channel != null)
            return string.Format("{0}/{1}", channel.Connections, channel.MaximumConnections);
        else
            return "Undefined";
    }

    public string ChannelType(Channel channel)
    {
        if (channel != null)
            switch (channel.Type)
            {
                case ServerType.RPG:
                    return "Survival/RPG";
                case ServerType.BattleRoyale:
                    return "Battle Royale";
                default:
                    return "Unknown";
            }
        else
            return "Undefined";
    }

    public void Selected()
    {
        CanConnect.Value = Channels.SelectedIndex > -1;
    }

    public void DSelected()
    {
        CanConnect.Value = Channels.SelectedIndex > -1;
    }

    public void UpdateList(Channel[] Channels)
    {
        this.Channels.Clear();
        this.Channels.AddRange(Channels);
    }

    public void Connect()
    {
        var channel = Channels.SelectedItem;

        if (Application.client != null)
            Application.client.Socket.Disconnect();

        var client = SingletonFactory.GetSingleton<PiAuthClient>();
        IGameClient gameClient = null;

        switch(channel.Type)
        {
            case ServerType.RPG:
                gameClient = new PiRPGClient();
                break;
            case ServerType.BattleRoyale:
                gameClient = new PiBRClient();
                break;
        }
        Application.client = gameClient;

        gameClient.Socket.EndPoint = new IPEndPoint(client.Socket.EndPoint.Address, channel.Port);
        if (!gameClient.Socket.Connect())
            Status.Value = "Falha ao conectar com o canal!";
        else
        {
            Status.Value = "Carregando pools...";
            WorldControl.PreparePool(channel.MaximumConnections);

            Status.Value = "Conectado! Aguardando personagens...";
            gameClient.Socket.Send(new SendCharactersRequest());
        }
    }

    public void SwitchToMenu()
    {
        var Menu = FindObjectOfType<MainMenu>();
        Menu.SwitchToMenu();
    }
}