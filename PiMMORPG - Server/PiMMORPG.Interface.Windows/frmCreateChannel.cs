using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PiMMORPG.Interface.Windows
{
    public partial class FrmCreateChannel : Form
    {
        public string Channel { get { return txtUsername.Text; } set { txtUsername.Text = value; } }
        public int Port { get { return (int)nuPort.Value; } set { nuPort.Value = value; } }
        public int MaxConnections { get { return (int)nMaxConnections.Value; } set { nMaxConnections.Value = value; } }
        public bool IsPvp { get { return cbPVP.Checked; } set { cbPVP.Checked = value; } }

        public FrmCreateChannel()
        {
            InitializeComponent();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Channel = string.Empty;
            nMaxConnections.Value = nMaxConnections.Minimum;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string message = string.Empty;
            if (Channel.Length == 0 || string.IsNullOrWhiteSpace(Channel))
                message = "Você deixou o nome de canal em branco!";

            if (message == string.Empty)
                DialogResult = DialogResult.OK;
            else
                MessageBox.Show(this, message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
    }
}
