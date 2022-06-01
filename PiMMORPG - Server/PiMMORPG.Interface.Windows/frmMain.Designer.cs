namespace PiMMORPG.Interface.Windows
{
    partial class FrmMain
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.stStatus = new System.Windows.Forms.StatusStrip();
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tpPrincipal = new System.Windows.Forms.TabPage();
            this.tlpPrincipal = new System.Windows.Forms.TableLayoutPanel();
            this.btnCreateAccount = new System.Windows.Forms.Button();
            this.btnOpenServer = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tpChannels = new System.Windows.Forms.TabPage();
            this.tlpChannels = new System.Windows.Forms.TableLayoutPanel();
            this.btnRegisterChannel = new System.Windows.Forms.Button();
            this.btnUpdateChannel = new System.Windows.Forms.Button();
            this.btnDeleteChannels = new System.Windows.Forms.Button();
            this.lvChannels = new System.Windows.Forms.ListView();
            this.chChannelStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chChannelID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chChannelName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chChannelPort = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chChannelMaxConnections = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chChannelPVP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.StateImageList = new System.Windows.Forms.ImageList(this.components);
            this.tpUsers = new System.Windows.Forms.TabPage();
            this.tlpUsers = new System.Windows.Forms.TableLayoutPanel();
            this.btnDisconnectSelectedUsers = new System.Windows.Forms.Button();
            this.btnSendMessage = new System.Windows.Forms.Button();
            this.btnUSerTemp = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.chEndPoint = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chUsername = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tpLogs = new System.Windows.Forms.TabPage();
            this.txtLogs = new System.Windows.Forms.RichTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.arquivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gerarListaDeAtualizaçãoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tcMain.SuspendLayout();
            this.tpPrincipal.SuspendLayout();
            this.tlpPrincipal.SuspendLayout();
            this.tpChannels.SuspendLayout();
            this.tlpChannels.SuspendLayout();
            this.tpUsers.SuspendLayout();
            this.tlpUsers.SuspendLayout();
            this.tpLogs.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // stStatus
            // 
            this.stStatus.Location = new System.Drawing.Point(0, 475);
            this.stStatus.Name = "stStatus";
            this.stStatus.Size = new System.Drawing.Size(897, 22);
            this.stStatus.TabIndex = 0;
            this.stStatus.Text = "statusStrip1";
            // 
            // tcMain
            // 
            this.tcMain.Controls.Add(this.tpPrincipal);
            this.tcMain.Controls.Add(this.tpChannels);
            this.tcMain.Controls.Add(this.tpUsers);
            this.tcMain.Controls.Add(this.tpLogs);
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMain.Location = new System.Drawing.Point(0, 24);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(897, 451);
            this.tcMain.TabIndex = 1;
            // 
            // tpPrincipal
            // 
            this.tpPrincipal.Controls.Add(this.tlpPrincipal);
            this.tpPrincipal.Location = new System.Drawing.Point(4, 22);
            this.tpPrincipal.Name = "tpPrincipal";
            this.tpPrincipal.Padding = new System.Windows.Forms.Padding(3);
            this.tpPrincipal.Size = new System.Drawing.Size(889, 425);
            this.tpPrincipal.TabIndex = 0;
            this.tpPrincipal.Text = "Principal";
            this.tpPrincipal.UseVisualStyleBackColor = true;
            // 
            // tlpPrincipal
            // 
            this.tlpPrincipal.ColumnCount = 3;
            this.tlpPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tlpPrincipal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tlpPrincipal.Controls.Add(this.btnCreateAccount, 0, 2);
            this.tlpPrincipal.Controls.Add(this.btnOpenServer, 0, 0);
            this.tlpPrincipal.Controls.Add(this.btnClose, 1, 0);
            this.tlpPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPrincipal.Location = new System.Drawing.Point(3, 3);
            this.tlpPrincipal.Name = "tlpPrincipal";
            this.tlpPrincipal.RowCount = 3;
            this.tlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpPrincipal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpPrincipal.Size = new System.Drawing.Size(883, 419);
            this.tlpPrincipal.TabIndex = 0;
            // 
            // btnCreateAccount
            // 
            this.btnCreateAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCreateAccount.Enabled = false;
            this.btnCreateAccount.Location = new System.Drawing.Point(15, 293);
            this.btnCreateAccount.Margin = new System.Windows.Forms.Padding(15);
            this.btnCreateAccount.Name = "btnCreateAccount";
            this.btnCreateAccount.Size = new System.Drawing.Size(264, 111);
            this.btnCreateAccount.TabIndex = 2;
            this.btnCreateAccount.Text = "Create account";
            this.btnCreateAccount.UseVisualStyleBackColor = true;
            this.btnCreateAccount.Click += new System.EventHandler(this.btnCreateAccount_Click);
            // 
            // btnOpenServer
            // 
            this.btnOpenServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenServer.Location = new System.Drawing.Point(15, 15);
            this.btnOpenServer.Margin = new System.Windows.Forms.Padding(15);
            this.btnOpenServer.Name = "btnOpenServer";
            this.btnOpenServer.Size = new System.Drawing.Size(264, 109);
            this.btnOpenServer.TabIndex = 0;
            this.btnOpenServer.Text = "Open server";
            this.btnOpenServer.UseVisualStyleBackColor = true;
            this.btnOpenServer.Click += new System.EventHandler(this.btnOpenServer_Click);
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.Enabled = false;
            this.btnClose.Location = new System.Drawing.Point(309, 15);
            this.btnClose.Margin = new System.Windows.Forms.Padding(15);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(264, 109);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close server";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tpChannels
            // 
            this.tpChannels.Controls.Add(this.tlpChannels);
            this.tpChannels.Location = new System.Drawing.Point(4, 22);
            this.tpChannels.Name = "tpChannels";
            this.tpChannels.Padding = new System.Windows.Forms.Padding(3);
            this.tpChannels.Size = new System.Drawing.Size(889, 425);
            this.tpChannels.TabIndex = 3;
            this.tpChannels.Text = "Canais";
            this.tpChannels.UseVisualStyleBackColor = true;
            // 
            // tlpChannels
            // 
            this.tlpChannels.ColumnCount = 3;
            this.tlpChannels.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpChannels.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpChannels.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpChannels.Controls.Add(this.btnRegisterChannel, 0, 1);
            this.tlpChannels.Controls.Add(this.btnUpdateChannel, 1, 1);
            this.tlpChannels.Controls.Add(this.btnDeleteChannels, 2, 1);
            this.tlpChannels.Controls.Add(this.lvChannels, 0, 0);
            this.tlpChannels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpChannels.Enabled = false;
            this.tlpChannels.Location = new System.Drawing.Point(3, 3);
            this.tlpChannels.Name = "tlpChannels";
            this.tlpChannels.RowCount = 2;
            this.tlpChannels.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85F));
            this.tlpChannels.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tlpChannels.Size = new System.Drawing.Size(883, 419);
            this.tlpChannels.TabIndex = 1;
            // 
            // btnRegisterChannel
            // 
            this.btnRegisterChannel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRegisterChannel.Location = new System.Drawing.Point(3, 359);
            this.btnRegisterChannel.Name = "btnRegisterChannel";
            this.btnRegisterChannel.Size = new System.Drawing.Size(288, 57);
            this.btnRegisterChannel.TabIndex = 0;
            this.btnRegisterChannel.Text = "Criar canal";
            this.btnRegisterChannel.UseVisualStyleBackColor = true;
            this.btnRegisterChannel.Click += new System.EventHandler(this.btnRegisterChannel_Click);
            // 
            // btnUpdateChannel
            // 
            this.btnUpdateChannel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUpdateChannel.Location = new System.Drawing.Point(297, 359);
            this.btnUpdateChannel.Name = "btnUpdateChannel";
            this.btnUpdateChannel.Size = new System.Drawing.Size(288, 57);
            this.btnUpdateChannel.TabIndex = 1;
            this.btnUpdateChannel.Text = "Atualizar canal";
            this.btnUpdateChannel.UseVisualStyleBackColor = true;
            // 
            // btnDeleteChannels
            // 
            this.btnDeleteChannels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDeleteChannels.Location = new System.Drawing.Point(591, 359);
            this.btnDeleteChannels.Name = "btnDeleteChannels";
            this.btnDeleteChannels.Size = new System.Drawing.Size(289, 57);
            this.btnDeleteChannels.TabIndex = 2;
            this.btnDeleteChannels.Text = "Deletar canais selecionados";
            this.btnDeleteChannels.UseVisualStyleBackColor = true;
            // 
            // lvChannels
            // 
            this.lvChannels.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chChannelStatus,
            this.chChannelID,
            this.chChannelName,
            this.chChannelPort,
            this.chChannelMaxConnections,
            this.chChannelPVP});
            this.tlpChannels.SetColumnSpan(this.lvChannels, 3);
            this.lvChannels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvChannels.FullRowSelect = true;
            this.lvChannels.GridLines = true;
            this.lvChannels.LargeImageList = this.StateImageList;
            this.lvChannels.Location = new System.Drawing.Point(3, 3);
            this.lvChannels.Name = "lvChannels";
            this.lvChannels.Size = new System.Drawing.Size(877, 350);
            this.lvChannels.SmallImageList = this.StateImageList;
            this.lvChannels.StateImageList = this.StateImageList;
            this.lvChannels.TabIndex = 3;
            this.lvChannels.UseCompatibleStateImageBehavior = false;
            this.lvChannels.View = System.Windows.Forms.View.Details;
            this.lvChannels.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvChannels_KeyDown);
            this.lvChannels.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvChannels_MouseDoubleClick);
            // 
            // chChannelStatus
            // 
            this.chChannelStatus.Text = "";
            this.chChannelStatus.Width = 32;
            // 
            // chChannelID
            // 
            this.chChannelID.Text = "ID";
            // 
            // chChannelName
            // 
            this.chChannelName.Text = "Nome";
            this.chChannelName.Width = 160;
            // 
            // chChannelPort
            // 
            this.chChannelPort.Text = "Porta";
            this.chChannelPort.Width = 80;
            // 
            // chChannelMaxConnections
            // 
            this.chChannelMaxConnections.Text = "Conexões";
            this.chChannelMaxConnections.Width = 80;
            // 
            // chChannelPVP
            // 
            this.chChannelPVP.Text = "PVP?";
            // 
            // StateImageList
            // 
            this.StateImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("StateImageList.ImageStream")));
            this.StateImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.StateImageList.Images.SetKeyName(0, "Online Icon");
            this.StateImageList.Images.SetKeyName(1, "Offline Icon");
            // 
            // tpUsers
            // 
            this.tpUsers.Controls.Add(this.tlpUsers);
            this.tpUsers.Location = new System.Drawing.Point(4, 22);
            this.tpUsers.Name = "tpUsers";
            this.tpUsers.Padding = new System.Windows.Forms.Padding(3);
            this.tpUsers.Size = new System.Drawing.Size(889, 425);
            this.tpUsers.TabIndex = 2;
            this.tpUsers.Text = "Users";
            this.tpUsers.UseVisualStyleBackColor = true;
            // 
            // tlpUsers
            // 
            this.tlpUsers.ColumnCount = 3;
            this.tlpUsers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpUsers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpUsers.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpUsers.Controls.Add(this.btnDisconnectSelectedUsers, 0, 1);
            this.tlpUsers.Controls.Add(this.btnSendMessage, 1, 1);
            this.tlpUsers.Controls.Add(this.btnUSerTemp, 2, 1);
            this.tlpUsers.Controls.Add(this.listView1, 0, 0);
            this.tlpUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpUsers.Location = new System.Drawing.Point(3, 3);
            this.tlpUsers.Name = "tlpUsers";
            this.tlpUsers.RowCount = 2;
            this.tlpUsers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85F));
            this.tlpUsers.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tlpUsers.Size = new System.Drawing.Size(883, 419);
            this.tlpUsers.TabIndex = 0;
            // 
            // btnDisconnectSelectedUsers
            // 
            this.btnDisconnectSelectedUsers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDisconnectSelectedUsers.Location = new System.Drawing.Point(3, 359);
            this.btnDisconnectSelectedUsers.Name = "btnDisconnectSelectedUsers";
            this.btnDisconnectSelectedUsers.Size = new System.Drawing.Size(288, 57);
            this.btnDisconnectSelectedUsers.TabIndex = 0;
            this.btnDisconnectSelectedUsers.Text = "Desconectar usuários selecionados";
            this.btnDisconnectSelectedUsers.UseVisualStyleBackColor = true;
            // 
            // btnSendMessage
            // 
            this.btnSendMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSendMessage.Location = new System.Drawing.Point(297, 359);
            this.btnSendMessage.Name = "btnSendMessage";
            this.btnSendMessage.Size = new System.Drawing.Size(288, 57);
            this.btnSendMessage.TabIndex = 1;
            this.btnSendMessage.Text = "Enviar mensagem aos usuários selecionados";
            this.btnSendMessage.UseVisualStyleBackColor = true;
            // 
            // btnUSerTemp
            // 
            this.btnUSerTemp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUSerTemp.Enabled = false;
            this.btnUSerTemp.Location = new System.Drawing.Point(591, 359);
            this.btnUSerTemp.Name = "btnUSerTemp";
            this.btnUSerTemp.Size = new System.Drawing.Size(289, 57);
            this.btnUSerTemp.TabIndex = 2;
            this.btnUSerTemp.Text = "Disabled";
            this.btnUSerTemp.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chEndPoint,
            this.chUsername});
            this.tlpUsers.SetColumnSpan(this.listView1, 3);
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(3, 3);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(877, 350);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // chEndPoint
            // 
            this.chEndPoint.Text = "IP";
            this.chEndPoint.Width = 130;
            // 
            // chUsername
            // 
            this.chUsername.Text = "Username";
            this.chUsername.Width = 130;
            // 
            // tpLogs
            // 
            this.tpLogs.Controls.Add(this.txtLogs);
            this.tpLogs.Location = new System.Drawing.Point(4, 22);
            this.tpLogs.Name = "tpLogs";
            this.tpLogs.Padding = new System.Windows.Forms.Padding(3);
            this.tpLogs.Size = new System.Drawing.Size(889, 425);
            this.tpLogs.TabIndex = 1;
            this.tpLogs.Text = "Logs";
            this.tpLogs.UseVisualStyleBackColor = true;
            // 
            // txtLogs
            // 
            this.txtLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLogs.Location = new System.Drawing.Point(3, 3);
            this.txtLogs.Name = "txtLogs";
            this.txtLogs.ReadOnly = true;
            this.txtLogs.Size = new System.Drawing.Size(883, 419);
            this.txtLogs.TabIndex = 0;
            this.txtLogs.Text = "";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.arquivoToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(897, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // arquivoToolStripMenuItem
            // 
            this.arquivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gerarListaDeAtualizaçãoToolStripMenuItem});
            this.arquivoToolStripMenuItem.Name = "arquivoToolStripMenuItem";
            this.arquivoToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.arquivoToolStripMenuItem.Text = "Arquivo";
            // 
            // gerarListaDeAtualizaçãoToolStripMenuItem
            // 
            this.gerarListaDeAtualizaçãoToolStripMenuItem.Name = "gerarListaDeAtualizaçãoToolStripMenuItem";
            this.gerarListaDeAtualizaçãoToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.gerarListaDeAtualizaçãoToolStripMenuItem.Text = "Gerar lista de atualização";
            this.gerarListaDeAtualizaçãoToolStripMenuItem.Click += new System.EventHandler(this.gerarListaDeAtualizaçãoToolStripMenuItem_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(897, 497);
            this.Controls.Add(this.tcMain);
            this.Controls.Add(this.stStatus);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PiMMORPG - Windows Interface";
            this.tcMain.ResumeLayout(false);
            this.tpPrincipal.ResumeLayout(false);
            this.tlpPrincipal.ResumeLayout(false);
            this.tpChannels.ResumeLayout(false);
            this.tlpChannels.ResumeLayout(false);
            this.tpUsers.ResumeLayout(false);
            this.tlpUsers.ResumeLayout(false);
            this.tpLogs.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip stStatus;
        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tpPrincipal;
        private System.Windows.Forms.TabPage tpLogs;
        private System.Windows.Forms.TableLayoutPanel tlpPrincipal;
        private System.Windows.Forms.Button btnOpenServer;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.RichTextBox txtLogs;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem arquivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gerarListaDeAtualizaçãoToolStripMenuItem;
        private System.Windows.Forms.TabPage tpUsers;
        private System.Windows.Forms.TableLayoutPanel tlpUsers;
        private System.Windows.Forms.Button btnDisconnectSelectedUsers;
        private System.Windows.Forms.Button btnSendMessage;
        private System.Windows.Forms.Button btnUSerTemp;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader chEndPoint;
        private System.Windows.Forms.ColumnHeader chUsername;
        private System.Windows.Forms.Button btnCreateAccount;
        private System.Windows.Forms.TabPage tpChannels;
        private System.Windows.Forms.TableLayoutPanel tlpChannels;
        private System.Windows.Forms.Button btnRegisterChannel;
        private System.Windows.Forms.Button btnUpdateChannel;
        private System.Windows.Forms.Button btnDeleteChannels;
        private System.Windows.Forms.ListView lvChannels;
        private System.Windows.Forms.ColumnHeader chChannelStatus;
        private System.Windows.Forms.ColumnHeader chChannelID;
        private System.Windows.Forms.ColumnHeader chChannelName;
        private System.Windows.Forms.ColumnHeader chChannelPort;
        private System.Windows.Forms.ColumnHeader chChannelMaxConnections;
        private System.Windows.Forms.ColumnHeader chChannelPVP;
        private System.Windows.Forms.ImageList StateImageList;
    }
}

