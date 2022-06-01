using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Base.Data.Abstracts;

namespace Server.Configuration
{
    public class PortsConfiguration : XMLConfiguration
    {
        public override bool Secure { get { return true; } }
        public override string Filename
        {
            get
            {
                return "ports.config";
            }
        }

        public override void WriteDefaults()
        {
            GatePort = 2000;
            AuthPort = 2005;
            DataPort = 2030;
        }

        public static int GatePort { get; set; }
        public static int AuthPort { get; set; }
        public static int DataPort { get; set; }
    }
}