using Base.Factories;
using Game.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace Game.Service
{
    public partial class frmCommand : Form
    {
        public frmCommand()
        {
            InitializeComponent();
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            var Factory = SingletonFactory.GetInstance<CommandFactory<GCommand>>();

            if (!Factory.ExecuteCommand(txtCommand.Text))
                MessageBox.Show(this, "Failed to execute command!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                txtCommand.Text = string.Empty;
        }
    }
}
