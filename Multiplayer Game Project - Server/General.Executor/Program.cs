using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Base.Factories;

namespace General.Executor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            LoggerFactory.OnLog += Server.Logger.ConsoleLogger.Fire;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}
