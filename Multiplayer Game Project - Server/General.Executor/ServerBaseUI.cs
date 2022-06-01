using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using IComponent = Base.Data.Interfaces.IComponent;

using Network.Data.Interfaces;
using Network.Data.EventArgs;

using Base.Factories;

namespace General.Executor
{
    public partial class ServerBaseUI : UserControl
    {
        private BindingList<IClientSocket> Clients;
        private BindingSource Source;

        public IComponent ServerComponent { get; set; }
        public IServerSocket Server { get; set; }

        public ServerBaseUI()
        {
            InitializeComponent();

            Clients = new BindingList<IClientSocket>();
            Source = new BindingSource(Clients, null);

            dgClients.DataSource = Source;
        }

        public void LoadEvents()
        {
            Server.Opened += Server_Opened;
            Server.Closed += Server_Closed;
            Server.ClientConnected += Server_ClientConnected;
        }

        private void Server_ClientConnected(object sender, ClientConnectedEventArgs e)
        {
            Clients = new BindingList<IClientSocket>(Server.Clients);
            Source = new BindingSource(Clients, null);
        }

        private void Client_Disconnected(object sender, ClientDisconnectedEventArgs e)
        {
            Clients = new BindingList<IClientSocket>(Server.Clients);
            Source = new BindingSource(Clients, null);
        }

        private void Server_Opened(object sender, ServerSocketEventArgs e)
        {
            btnOpen.Enabled = false;
            btnClose.Enabled = true;
            btnDisconnectAll.Enabled = true;
        }

        private void Server_Closed(object sender, ServerSocketEventArgs e)
        {
            btnOpen.Enabled = true;
            btnClose.Enabled = false;
            btnDisconnectAll.Enabled = false;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if(ComponentFactory.Enable(ServerComponent.GetType()))
            {
                
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (ComponentFactory.Disable(ServerComponent.GetType()))
            {

            }
        }

        private void btnDisconnectAll_Click(object sender, EventArgs e)
        {

        }
    }
}
