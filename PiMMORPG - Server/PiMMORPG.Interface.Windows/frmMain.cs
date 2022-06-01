using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows.Forms;

using tFramework.Enums;
using tFramework.Factories;
using tFramework.EventArgs;

using tFramework.Network;
using tFramework.Network.EventArgs;

using tFramework.DataDriver.Enums;
using tFramework.DataDriver.EventArgs;

namespace PiMMORPG.Interface.Windows
{
    using Models;
    using Client;
    using Server;
    using Server.Enums;
    using Server.Drivers;
    using tFramework.Helper;

    public partial class FrmMain : Form
    {
        PiServer _server;
        public FrmMain()
        {
            InitializeComponent();

            LoggerFactory.OnLog += LoggerFactory_OnLog;
            _server = SingletonFactory.GetSingleton<PiServer>();
            _server.OnOpened += Server_OnOpened;
            _server.OnClosed += Server_OnClosed;
            _server.OnConnected += Server_OnConnected;
            _server.OnDisconnected += Server_OnDisconnected;

            ChannelDriver.Refreshed += ChannelDriver_Refreshed;
        }

        private void ChannelDriver_Refreshed(object sender, CachedDriverRefreshEventArgs<Channel> e)
        {
            if (InvokeRequired)
                Invoke(new Action(() => ChannelDriver_Refreshed(sender, e)));
            else
            {
                foreach (var channel in e.Models)
                {
                    var index = lvChannels.Items.IndexOfKey(Convert.ToString(channel.Id));
                    if ((e.Operation == DriverOperation.Insert || e.Operation == DriverOperation.Loaded) && index <= -1)
                    {
                        var item = lvChannels.Items.Add(string.Empty);
                        item.Name = Convert.ToString(channel.Id);
                        item.ImageKey = "Offline Icon";

                        item.SubItems.Add(Convert.ToString(channel.Id));
                        item.SubItems.Add(channel.Name);
                        item.SubItems.Add(Convert.ToString(channel.Port));
                        item.SubItems.Add(Convert.ToString(channel.MaximumConnections));
                        item.SubItems.Add(channel.IsPvp ? "Sim" : "Não");
                    }
                    else if (e.Operation == DriverOperation.Update || index > -1)
                    {
                        var item = lvChannels.Items[index];
                        item.SubItems[1].Text = Convert.ToString(channel.Id);
                        item.SubItems[2].Text = channel.Name;
                        item.SubItems[3].Text = Convert.ToString(channel.Port);
                        item.SubItems[4].Text = Convert.ToString(channel.MaximumConnections);
                        item.SubItems[5].Text = channel.IsPvp ? "Sim" : "Não";
                    }
                    else if (e.Operation == DriverOperation.Delete)
                    {
                        if (index > -1)
                            lvChannels.Items.RemoveAt(index);
                    }
                }
            }
        }

        private void Server_OnOpened(object sender, BaseServerEventArgs<PiServer, TCPAsyncServer, PiClient, TCPAsyncClient> e)
        {
            if (InvokeRequired)
                Invoke(new Action(() => Server_OnOpened(sender, e)));
            else
            {
                btnOpenServer.Enabled = false;
                btnClose.Enabled = true;
                btnCreateAccount.Enabled = true;

                tlpChannels.Enabled = true;
            }
        }

        private void Server_OnClosed(object sender, BaseServerEventArgs<PiServer, TCPAsyncServer, PiClient, TCPAsyncClient> e)
        {
            if (InvokeRequired)
                Invoke(new Action(() => Server_OnClosed(sender, e)));
            else if(lvChannels != null && !lvChannels.IsDisposed)
            {
                btnOpenServer.Enabled = true;
                btnClose.Enabled = false;
                btnCreateAccount.Enabled = false;

                tlpChannels.Enabled = false;
                lvChannels.Items.Clear();
            }
        }

        private void Server_OnConnected(object sender, BaseClientEventArgs<PiClient, TCPAsyncClient> e)
        {
            if (InvokeRequired)
                Invoke(new Action(() => Server_OnConnected(sender, e)));
            else
            {
                var item = listView1.Items.Add(e.Client.Socket.EndPoint.ToString());
                item.Name = item.Text;
            }
        }

        private void Server_OnDisconnected(object sender, BaseClientEventArgs<PiClient, TCPAsyncClient> e)
        {
            if (InvokeRequired)
                Invoke(new Action(() => Server_OnDisconnected(sender, e)));
            else
            {
                var key = e.Client.Socket.EndPoint.ToString();
                listView1.Items.RemoveByKey(key);
            }
        }

        void AppendText(string text, Color color)
        {
            if (txtLogs != null)
            {
                if (InvokeRequired)
                    Invoke(new Action<string, Color>(AppendText), text, color);
                else if(!txtLogs.IsDisposed)
                {
                    txtLogs.SelectionStart = txtLogs.TextLength;
                    txtLogs.SelectionLength = 0;

                    txtLogs.SelectionColor = color;
                    txtLogs.AppendText(text);
                    txtLogs.SelectionColor = txtLogs.ForeColor;
                }
            }
        }

        private Point? _p;
        private void LoggerFactory_OnLog(object sender, LogEventArgs e)
        {
            if(_p == null)
                _p = new Point(txtLogs.Width, txtLogs.Height);
            
            var index = txtLogs.GetCharIndexFromPosition(_p.Value);
            var atMax = txtLogs.TextLength - 1 == index;
            
            AppendText($"[{DateTime.Now} - ", txtLogs.ForeColor);
            switch(e.Type)
            {
                case LogType.Information:
                    AppendText(e.Logger.Name, Color.DarkCyan);
                    break;
                case LogType.Success:
                    AppendText(e.Logger.Name, Color.DarkGreen);
                    break;
                case LogType.Warning:
                    AppendText(e.Logger.Name, Color.DarkGoldenrod);
                    break;
                case LogType.Error:
                    AppendText(e.Logger.Name, Color.DarkRed);
                    break;
                case LogType.Fatal:
                    AppendText(e.Logger.Name, Color.Red);
                    break;
            }
            AppendText($"]: {e.Message}" + Environment.NewLine, txtLogs.ForeColor);

            if (atMax)
            {
                txtLogs.SelectionStart = txtLogs.TextLength;
                txtLogs.ScrollToCaret();
            }
        }

        private void btnOpenServer_Click(object sender, EventArgs e)
        {
            var message = string.Empty;

            if(!ComponentFactory.Enable<PiServer>())
            {
                switch(_server.LastResult)
                {
                    case OpenServerResult.ConfigurationFail:
                        message = "Falha ao carregar o arquivo de configuração!";
                        break;
                    case OpenServerResult.AccountDriverFail:
                        message = "Falha ao carregar o driver de contas!";
                        break;
                    case OpenServerResult.ChannelDriverFail:
                        message = "Falha ao carregar o driver de canais!";
                        break;
                    case OpenServerResult.ChannelsFail:
                        message = "Falha ao iniciar os canais!";
                        break;
                    case OpenServerResult.SocketFail:
                        message = "Falha ao iniciar o servidor!";
                        break;
                    case OpenServerResult.Unknown:
                        message = "Erro desconhecido!";
                        break;
                }
            }

            if (message != string.Empty)
                MessageBox.Show(this, message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if(!ComponentFactory.Disable<PiServer>())
                MessageBox.Show(this, "Falha ao fechar o servidor!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void gerarListaDeAtualizaçãoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new CommonOpenFileDialog())
            {
                dialog.RestoreDirectory = true;
                dialog.IsFolderPicker = true;

                if(dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    PiServer.GenerateFileList(dialog.FileName);
                }
            }
        }

        private void btnCreateAccount_Click(object sender, EventArgs e)
        {
            using (var dialog = new FrmCreateAccount())
            {
                if(dialog.ShowDialog() == DialogResult.OK)
                {
                    using (var Context = new AccountDriver())
                    {
                        var account = new Account
                        {
                            Username = dialog.Username,
                            Password = dialog.Password.CalculateMd5()
                        };
                        Context.AddModel(account);
                    }
                }
            }
        }

        private void btnRegisterChannel_Click(object sender, EventArgs e)
        {
            using (var dialog = new FrmCreateChannel())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var channel = new Channel
                    {
                        Name = dialog.Channel,
                        IsPvp = dialog.IsPvp,
                        Port = dialog.Port,
                        MaximumConnections = dialog.MaxConnections
                    };
                    ChannelDriver.AddModel(channel);
                }
            }
        }

        private void lvChannels_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var test = lvChannels.HitTest(e.Location);
            if(test != null)
            {
                var channel = _server.Channels.First(g => g.Channel.Name == test.Item.SubItems[2].Text);
                SwitchChannel(channel, test.Item);
            }
        }

        private void lvChannels_KeyDown(object sender, KeyEventArgs e)
        {
            if((e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return) && lvChannels.SelectedItems.Count > 0)
            {
                foreach (ListViewItem item in lvChannels.SelectedItems)
                {
                    var channel = _server.Channels.First(g => g.Channel.Name == item.SubItems[2].Text);
                    SwitchChannel(channel, item);
                }
            }
        }

        private void SwitchChannel(PiGameServer channel, ListViewItem item)
        {
            if (!ComponentFactory.IsEnabled(channel))
            {
                if (ComponentFactory.Enable(channel))
                {
                    item.ImageKey = "Online Icon";
                }
                else
                    MessageBox.Show(this, "Falha ao iniciar o canal!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                if (ComponentFactory.Disable(channel))
                {
                    item.ImageKey = "Offline Icon";
                }
                else
                    MessageBox.Show(this, "Falha ao iniciar o canal!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
    }
}