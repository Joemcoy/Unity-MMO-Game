using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Base.Data.Abstracts;
using Game.Data.Information;

namespace Server.Configuration
{
    public class GatesConfiguration : XMLConfiguration
    {
        public override bool Secure { get { return true; } }
        public override string Filename { get { return "gates.config"; } }

        public override void WriteDefaults()
        {
            Gates = new GateInfo[2]
            {
                new GateInfo { Name = "Servidor PVP", PVP = true, Port = 2010 },
                new GateInfo { Name = "Servidor PVE", PVP = false, Port = 2012 }
            };
        }

        public static GateInfo[] Gates { get; set; }
    }
}