namespace General.Executor
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tlpLayout = new System.Windows.Forms.TableLayoutPanel();
            this.tcServers = new System.Windows.Forms.TabControl();
            this.tpDataServer = new System.Windows.Forms.TabPage();
            this.uiDataServer = new General.Executor.ServerBaseUI();
            this.tpAuth = new System.Windows.Forms.TabPage();
            this.uiAuthServer = new General.Executor.ServerBaseUI();
            this.lblAbout = new System.Windows.Forms.Label();
            this.tpChatServer = new System.Windows.Forms.TabPage();
            this.uiChatServer = new General.Executor.ServerBaseUI();
            this.tlpLayout.SuspendLayout();
            this.tcServers.SuspendLayout();
            this.tpDataServer.SuspendLayout();
            this.tpAuth.SuspendLayout();
            this.tpChatServer.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpLayout
            // 
            this.tlpLayout.ColumnCount = 1;
            this.tlpLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpLayout.Controls.Add(this.tcServers, 0, 0);
            this.tlpLayout.Controls.Add(this.lblAbout, 0, 1);
            this.tlpLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpLayout.Location = new System.Drawing.Point(0, 0);
            this.tlpLayout.Name = "tlpLayout";
            this.tlpLayout.RowCount = 2;
            this.tlpLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 83.33334F));
            this.tlpLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlpLayout.Size = new System.Drawing.Size(784, 407);
            this.tlpLayout.TabIndex = 0;
            // 
            // tcServers
            // 
            this.tcServers.Controls.Add(this.tpDataServer);
            this.tcServers.Controls.Add(this.tpAuth);
            this.tcServers.Controls.Add(this.tpChatServer);
            this.tcServers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcServers.Location = new System.Drawing.Point(3, 3);
            this.tcServers.Name = "tcServers";
            this.tcServers.SelectedIndex = 0;
            this.tcServers.Size = new System.Drawing.Size(778, 333);
            this.tcServers.TabIndex = 0;
            // 
            // tpDataServer
            // 
            this.tpDataServer.Controls.Add(this.uiDataServer);
            this.tpDataServer.Location = new System.Drawing.Point(4, 22);
            this.tpDataServer.Name = "tpDataServer";
            this.tpDataServer.Padding = new System.Windows.Forms.Padding(3);
            this.tpDataServer.Size = new System.Drawing.Size(770, 307);
            this.tpDataServer.TabIndex = 0;
            this.tpDataServer.Text = "Data Server";
            this.tpDataServer.UseVisualStyleBackColor = true;
            // 
            // uiDataServer
            // 
            this.uiDataServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiDataServer.Location = new System.Drawing.Point(3, 3);
            this.uiDataServer.Name = "uiDataServer";
            this.uiDataServer.Server = null;
            this.uiDataServer.ServerComponent = null;
            this.uiDataServer.Size = new System.Drawing.Size(764, 301);
            this.uiDataServer.TabIndex = 0;
            // 
            // tpAuth
            // 
            this.tpAuth.Controls.Add(this.uiAuthServer);
            this.tpAuth.Location = new System.Drawing.Point(4, 22);
            this.tpAuth.Name = "tpAuth";
            this.tpAuth.Padding = new System.Windows.Forms.Padding(3);
            this.tpAuth.Size = new System.Drawing.Size(770, 307);
            this.tpAuth.TabIndex = 2;
            this.tpAuth.Text = "Auth Server";
            this.tpAuth.UseVisualStyleBackColor = true;
            // 
            // uiAuthServer
            // 
            this.uiAuthServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiAuthServer.Location = new System.Drawing.Point(3, 3);
            this.uiAuthServer.Name = "uiAuthServer";
            this.uiAuthServer.Server = null;
            this.uiAuthServer.ServerComponent = null;
            this.uiAuthServer.Size = new System.Drawing.Size(764, 301);
            this.uiAuthServer.TabIndex = 0;
            // 
            // lblAbout
            // 
            this.lblAbout.AutoSize = true;
            this.lblAbout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAbout.Location = new System.Drawing.Point(5, 344);
            this.lblAbout.Margin = new System.Windows.Forms.Padding(5);
            this.lblAbout.Name = "lblAbout";
            this.lblAbout.Size = new System.Drawing.Size(774, 58);
            this.lblAbout.TabIndex = 1;
            this.lblAbout.Text = "4FunGames - RPG Project - General Executor - Desenvolvido por Gabriel Oliveira Br" +
    "ito e Sidnei Oliveira";
            this.lblAbout.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tpChatServer
            // 
            this.tpChatServer.Controls.Add(this.uiChatServer);
            this.tpChatServer.Location = new System.Drawing.Point(4, 22);
            this.tpChatServer.Name = "tpChatServer";
            this.tpChatServer.Padding = new System.Windows.Forms.Padding(3);
            this.tpChatServer.Size = new System.Drawing.Size(770, 307);
            this.tpChatServer.TabIndex = 3;
            this.tpChatServer.Text = "Chat Server";
            this.tpChatServer.UseVisualStyleBackColor = true;
            // 
            // uiChatServer
            // 
            this.uiChatServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiChatServer.Location = new System.Drawing.Point(3, 3);
            this.uiChatServer.Name = "uiChatServer";
            this.uiChatServer.Server = null;
            this.uiChatServer.ServerComponent = null;
            this.uiChatServer.Size = new System.Drawing.Size(764, 301);
            this.uiChatServer.TabIndex = 0;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 407);
            this.Controls.Add(this.tlpLayout);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmMain";
            this.Text = "4Fun Games - General Executor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.tlpLayout.ResumeLayout(false);
            this.tlpLayout.PerformLayout();
            this.tcServers.ResumeLayout(false);
            this.tpDataServer.ResumeLayout(false);
            this.tpAuth.ResumeLayout(false);
            this.tpChatServer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpLayout;
        private System.Windows.Forms.TabControl tcServers;
        private System.Windows.Forms.TabPage tpDataServer;
        private System.Windows.Forms.Label lblAbout;
        private ServerBaseUI uiDataServer;
        private System.Windows.Forms.TabPage tpAuth;
        private ServerBaseUI uiAuthServer;
        private System.Windows.Forms.TabPage tpChatServer;
        private ServerBaseUI uiChatServer;
    }
}

