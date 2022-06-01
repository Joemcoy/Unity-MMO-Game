using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Data.Abstracts;

namespace Game.Data.Information
{
    public class GateInfo : APacketWrapper
    {
        public string Address { get; set; }
        public int Port { get; set; }
        public string Name { get; set; }
        public bool PVP { get; set; }
        public int MaximumClients { get; set; }
        public int ClientCount { get; set; }

        public GateInfo()
        {
            MaximumClients = 10;
            Name = string.Empty;
        }

        public GateInfo(int Port, string Name) : this(Port, Name, false)
        {
            
        }

        public GateInfo(int Port, string Name, bool PVP)
        {
            this.Port = Port;
            this.Name = Name;
            this.PVP = PVP;
        }
    }
}
