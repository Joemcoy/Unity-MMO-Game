using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;

using Network.Data.Interfaces;
using Network.Data.EventArgs;

using Base.Factories;
using Game.Server;
using Base.Helpers;
using Base.Data.EventArgs;

namespace Game.Service
{
    public partial class frmMain : Form
    {
        List<IClientSocket> Clients;

        public frmMain()
        {
            InitializeComponent();

            EventHelper.EventFired += EventHelper_EventFired;
            Clients = new List<IClientSocket>();

            var GameServer = SingletonFactory.GetInstance<GameServer>();
            GameServer.Socket.ClientConnected += Server_ClientConnected;
        }

        private void EventHelper_EventFired(object sender, SafeEventArgs e)
        {
            if (InvokeRequired)
                Invoke(e.Action);
            else
                e.Action();
        }

        private void Server_ClientConnected(object sender, ClientConnectedEventArgs e)
        {
            e.Client.Disconnected += Client_Disconnected;
            Clients.Add(e.Client);
        }

        private void Client_Disconnected(object sender, ClientDisconnectedEventArgs e)
        {
            Clients.Remove(e.Client);
        }
        
        private void lvUpdaterTimer_Tick(object sender, EventArgs e)
        {
            lvClients.BeginUpdate();
            lvClients.Items.Clear();
            lvClients.Items.AddRange(Clients.Select(CreateItem).ToArray());
            lvClients.EndUpdate();
        }

        ListViewItem CreateItem(IClientSocket Socket)
        {
            var Item = new ListViewItem(Socket.EndPoint.ToString());
            Item.SubItems.Add(string.Format("{0} ms", Socket.Ping));

            return Item;
        }
    }
}
