namespace PiMMORPG.Interface.Windows
{
    partial class FrmCreateChannel
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
            this.btnClear = new System.Windows.Forms.Button();
            this.btnRegister = new System.Windows.Forms.Button();
            this.lblChannelName = new System.Windows.Forms.Label();
            this.lblMaxConnections = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.nMaxConnections = new System.Windows.Forms.NumericUpDown();
            this.cbPVP = new PiMMORPG.Interface.Windows.Controls.YesNoCheckbox();
            this.lblPVP = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nuPort = new System.Windows.Forms.NumericUpDown();
            this.tlpLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nMaxConnections)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuPort)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpLayout
            // 
            this.tlpLayout.ColumnCount = 2;
            this.tlpLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpLayout.Controls.Add(this.label1, 0, 2);
            this.tlpLayout.Controls.Add(this.nuPort, 0, 2);
            this.tlpLayout.Controls.Add(this.lblPVP, 0, 3);
            this.tlpLayout.Controls.Add(this.btnClear, 1, 4);
            this.tlpLayout.Controls.Add(this.btnRegister, 0, 4);
            this.tlpLayout.Controls.Add(this.lblChannelName, 0, 0);
            this.tlpLayout.Controls.Add(this.lblMaxConnections, 0, 1);
            this.tlpLayout.Controls.Add(this.txtUsername, 1, 0);
            this.tlpLayout.Controls.Add(this.nMaxConnections, 1, 1);
            this.tlpLayout.Controls.Add(this.cbPVP, 1, 3);
            this.tlpLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpLayout.Location = new System.Drawing.Point(0, 0);
            this.tlpLayout.Name = "tlpLayout";
            this.tlpLayout.RowCount = 5;
            this.tlpLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpLayout.Size = new System.Drawing.Size(551, 227);
            this.tlpLayout.TabIndex = 0;
            // 
            // btnClear
            // 
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClear.Location = new System.Drawing.Point(285, 190);
            this.btnClear.Margin = new System.Windows.Forms.Padding(10);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(256, 27);
            this.btnClear.TabIndex = 0;
            this.btnClear.Text = "Limpar campos";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnRegister
            // 
            this.btnRegister.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRegister.Location = new System.Drawing.Point(10, 190);
            this.btnRegister.Margin = new System.Windows.Forms.Padding(10);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(255, 27);
            this.btnRegister.TabIndex = 1;
            this.btnRegister.Text = "Registrar";
            this.btnRegister.UseVisualStyleBackColor = true;
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
            // 
            // lblChannelName
            // 
            this.lblChannelName.BackColor = System.Drawing.SystemColors.Control;
            this.lblChannelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblChannelName.Location = new System.Drawing.Point(15, 15);
            this.lblChannelName.Margin = new System.Windows.Forms.Padding(15);
            this.lblChannelName.Name = "lblChannelName";
            this.lblChannelName.Size = new System.Drawing.Size(245, 15);
            this.lblChannelName.TabIndex = 2;
            this.lblChannelName.Text = "Nome do canal";
            this.lblChannelName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMaxConnections
            // 
            this.lblMaxConnections.BackColor = System.Drawing.SystemColors.Control;
            this.lblMaxConnections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMaxConnections.Location = new System.Drawing.Point(15, 60);
            this.lblMaxConnections.Margin = new System.Windows.Forms.Padding(15);
            this.lblMaxConnections.Name = "lblMaxConnections";
            this.lblMaxConnections.Size = new System.Drawing.Size(245, 15);
            this.lblMaxConnections.TabIndex = 3;
            this.lblMaxConnections.Text = "Máximo de conexões";
            this.lblMaxConnections.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtUsername
            // 
            this.txtUsername.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUsername.Location = new System.Drawing.Point(290, 12);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(15, 12, 15, 12);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(246, 20);
            this.txtUsername.TabIndex = 4;
            // 
            // nMaxConnections
            // 
            this.nMaxConnections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nMaxConnections.Location = new System.Drawing.Point(290, 57);
            this.nMaxConnections.Margin = new System.Windows.Forms.Padding(15, 12, 15, 12);
            this.nMaxConnections.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nMaxConnections.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nMaxConnections.Name = "nMaxConnections";
            this.nMaxConnections.Size = new System.Drawing.Size(246, 20);
            this.nMaxConnections.TabIndex = 5;
            this.nMaxConnections.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // cbPVP
            // 
            this.cbPVP.AutoSize = true;
            this.cbPVP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbPVP.Location = new System.Drawing.Point(400, 138);
            this.cbPVP.Margin = new System.Windows.Forms.Padding(125, 3, 3, 3);
            this.cbPVP.Name = "cbPVP";
            this.cbPVP.NoString = "No";
            this.cbPVP.Size = new System.Drawing.Size(148, 39);
            this.cbPVP.TabIndex = 6;
            this.cbPVP.Text = "No";
            this.cbPVP.UseVisualStyleBackColor = true;
            this.cbPVP.YesString = "Yes";
            // 
            // lblPVP
            // 
            this.lblPVP.BackColor = System.Drawing.SystemColors.Control;
            this.lblPVP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPVP.Location = new System.Drawing.Point(15, 150);
            this.lblPVP.Margin = new System.Windows.Forms.Padding(15);
            this.lblPVP.Name = "lblPVP";
            this.lblPVP.Size = new System.Drawing.Size(245, 15);
            this.lblPVP.TabIndex = 7;
            this.lblPVP.Text = "Canal PVP";
            this.lblPVP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(15, 105);
            this.label1.Margin = new System.Windows.Forms.Padding(15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(245, 15);
            this.label1.TabIndex = 8;
            this.label1.Text = "Porta";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nuPort
            // 
            this.nuPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nuPort.Location = new System.Drawing.Point(290, 102);
            this.nuPort.Margin = new System.Windows.Forms.Padding(15, 12, 15, 12);
            this.nuPort.Maximum = new decimal(new int[] {
            49151,
            0,
            0,
            0});
            this.nuPort.Minimum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.nuPort.Name = "nuPort";
            this.nuPort.Size = new System.Drawing.Size(246, 20);
            this.nuPort.TabIndex = 9;
            this.nuPort.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            // 
            // frmCreateChannel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 227);
            this.Controls.Add(this.tlpLayout);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmCreateChannel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PiMMORPG - Criar canal";
            this.tlpLayout.ResumeLayout(false);
            this.tlpLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nMaxConnections)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuPort)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpLayout;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Label lblChannelName;
        private System.Windows.Forms.Label lblMaxConnections;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.NumericUpDown nMaxConnections;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nuPort;
        private System.Windows.Forms.Label lblPVP;
        private Controls.YesNoCheckbox cbPVP;
    }
}