using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Base.Factories;

using Data.Server;
using Auth.Server;
using Game.Server;

using Base.Data.EventArgs;
using System.Runtime.InteropServices;

namespace General.Executor
{
    public partial class frmMain : Form
    {
        DataServer Data;
        AuthServer Auth;
        GameServer Game;

        public frmMain()
        {
            InitializeComponent();
            
            Disposed += FrmMain_Disposed;

            Data = SingletonFactory.GetInstance<DataServer>();
            Auth = SingletonFactory.GetInstance<AuthServer>();

            uiDataServer.ServerComponent = Data;
            uiDataServer.Server = Data.Socket;
            uiDataServer.LoadEvents();

            uiAuthServer.ServerComponent = Auth;
            uiAuthServer.Server = Auth.Socket;
            uiAuthServer.LoadEvents();
        }

        private void FrmMain_Disposed(object sender, EventArgs e)
        {
            
        }
        
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            SingletonFactory.DestroyAll();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
