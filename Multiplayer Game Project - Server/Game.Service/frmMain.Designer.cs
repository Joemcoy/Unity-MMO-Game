namespace Game.Service
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
            this.components = new System.ComponentModel.Container();
            this.tcServer = new System.Windows.Forms.TabControl();
            this.tpClients = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lvClients = new System.Windows.Forms.ListView();
            this.chClientEP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chClientPing = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvUpdaterTimer = new System.Windows.Forms.Timer(this.components);
            this.tcServer.SuspendLayout();
            this.tpClients.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcServer
            // 
            this.tcServer.Controls.Add(this.tpClients);
            this.tcServer.Controls.Add(this.tabPage2);
            this.tcServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcServer.Location = new System.Drawing.Point(0, 0);
            this.tcServer.Name = "tcServer";
            this.tcServer.SelectedIndex = 0;
            this.tcServer.Size = new System.Drawing.Size(643, 319);
            this.tcServer.TabIndex = 0;
            // 
            // tpClients
            // 
            this.tpClients.Controls.Add(this.lvClients);
            this.tpClients.Location = new System.Drawing.Point(4, 22);
            this.tpClients.Name = "tpClients";
            this.tpClients.Padding = new System.Windows.Forms.Padding(3);
            this.tpClients.Size = new System.Drawing.Size(635, 293);
            this.tpClients.TabIndex = 0;
            this.tpClients.Text = "Clients";
            this.tpClients.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(192, 74);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lvClients
            // 
            this.lvClients.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chClientEP,
            this.chClientPing});
            this.lvClients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvClients.FullRowSelect = true;
            this.lvClients.GridLines = true;
            this.lvClients.Location = new System.Drawing.Point(3, 3);
            this.lvClients.Margin = new System.Windows.Forms.Padding(5);
            this.lvClients.Name = "lvClients";
            this.lvClients.Size = new System.Drawing.Size(629, 287);
            this.lvClients.TabIndex = 0;
            this.lvClients.UseCompatibleStateImageBehavior = false;
            this.lvClients.View = System.Windows.Forms.View.Details;
            // 
            // chClientEP
            // 
            this.chClientEP.Text = "Client EndPoint";
            this.chClientEP.Width = 160;
            // 
            // chClientPing
            // 
            this.chClientPing.Text = "Ping";
            // 
            // lvUpdaterTimer
            // 
            this.lvUpdaterTimer.Enabled = true;
            this.lvUpdaterTimer.Tick += new System.EventHandler(this.lvUpdaterTimer_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 319);
            this.Controls.Add(this.tcServer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "4Fun Games - Game Server";
            this.tcServer.ResumeLayout(false);
            this.tpClients.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcServer;
        private System.Windows.Forms.TabPage tpClients;
        private System.Windows.Forms.ListView lvClients;
        private System.Windows.Forms.ColumnHeader chClientEP;
        private System.Windows.Forms.ColumnHeader chClientPing;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Timer lvUpdaterTimer;
    }
}