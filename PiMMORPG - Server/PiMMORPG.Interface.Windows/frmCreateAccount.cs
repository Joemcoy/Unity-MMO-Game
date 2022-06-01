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
    public partial class FrmCreateAccount : Form
    {
        public string Username { get { return txtUsername.Text; } set { txtUsername.Text = value; } }
        public string Password { get { return txtPassword.Text; } set { txtPassword.Text = value; } }

        public FrmCreateAccount()
        {
            InitializeComponent();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Username = string.Empty;
            Password = string.Empty;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string message = string.Empty;
            if (Username.Length == 0 || string.IsNullOrWhiteSpace(Username))
                message = "Você deixou o nome de usuário em branco!";
            else if(Password.Length == 0 || string.IsNullOrWhiteSpace(Password))
                message = "Você deixoua senha em branco!";

            if (message == string.Empty)
                DialogResult = DialogResult.OK;
            else
                MessageBox.Show(this, message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
    }
}
