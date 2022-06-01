using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Base.Data.Abstracts;

namespace Server.Configuration
{
    public class DatabaseConfiguration : XMLConfiguration
    {
        public override bool Secure { get { return true; } }
        public override string Filename
        {
            get
            {
                return "db.config";
            }
        }

        public override void WriteDefaults()
        {
            ConnectionString = "Server=localhost;Port=3306;Uid=root;Pwd=root;Pooling=true";
            Database = "mmoserver";
        }

        public static string ConnectionString { get; set; }
        public static string Database { get; set; }
    }
}