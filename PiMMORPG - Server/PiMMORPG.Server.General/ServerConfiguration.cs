using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using tFramework.Data.Interfaces;

namespace PiMMORPG.Server.General
{
    public class ServerConfiguration : IConfiguration
    {
        string IConfiguration.Filename => "server.cfg";
        bool IConfiguration.Secure => false;

        public string ChecksumMD5 { get; set; } = "";
        public int Port { get; set; } = 1793;
        public string ConnectionString { get; set; } = "Server=localhost;Database=piMMORPG;Uid=root;Pwd=root;";
        public int BattleRoyaleMap { get; set; }
    }
}