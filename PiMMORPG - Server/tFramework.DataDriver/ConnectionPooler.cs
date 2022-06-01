//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Data;
//using System.Data.Common;

//using System.Threading.Tasks;

//namespace tFramework.DataDriver
//{
//    public static class ConnectionPooler
//    {
//        static Dictionary<DbConnectionStringBuilder, DbConnection> connections;
//        static object syncLock;
//        static DbConnectionStringBuilder builder;

//        static ConnectionPooler()
//        {
//            connections = new Dictionary<DbConnectionStringBuilder, DbConnection>();
//            syncLock = new object();
//            builder = new DbConnectionStringBuilder();
//        }

//        static DbConnectionStringBuilder CreateBuilder<TConnection>()
//            where TConnection : DbConnection
//        {
//            var type = typeof(TConnection).Assembly.GetTypes().Where(t => typeof(DbConnectionStringBuilder).IsAssignableFrom(t) && t.Namespace == typeof(TConnection).Name).FirstOrDefault();
//            Factories.LoggerFactory.GetLogger(typeof(ConnectionPooler)).LogInfo(type == null ? "Default" : type.Name);

//            return type == null ? new DbConnectionStringBuilder() : Activator.CreateInstance(type) as DbConnectionStringBuilder;
//        }

//        static TConnection PreConnection<TConnection>(string connectionString)
//            where TConnection : DbConnection, new()
//        {
//            builder.Clear();
//            builder.ConnectionString = connectionString;

//            int total = builder.Count;
//            foreach (var connBuilder in connections.Keys)
//            {
//                int n = 0;
//                foreach (string key in connBuilder.Keys)
//                {
//                    if (builder.ContainsKey(key) && Convert.ToString(connBuilder[key]).Equals(Convert.ToString(builder[key])))
//                        n++;
//                }
//                if ((n * 100) / total >= 60)
//                {
//                    var con = connections[connBuilder];
//                    if (con.State == ConnectionState.Closed) con.Open();
//                    lock (con)
//                        return con as TConnection;
//                }
//            }

//            var conn = new TConnection();
//            conn.ConnectionString = connectionString;

//            return conn;
//        }

//        public static async Task<TConnection> GetConnectionAsync<TConnection>(string connectionString)
//            where TConnection : DbConnection, new()
//        {
//            var conn = PreConnection<TConnection>(connectionString);
//            await conn.OpenAsync();

//            lock (syncLock)
//                connections[new DbConnectionStringBuilder() { ConnectionString = connectionString }] = conn;
//            return conn;
//            //}
//        }

//        public static void ClearConnections()
//        {
//            lock(syncLock)
//            {
//                foreach (var conn in connections.Values)
//                    conn.Close();
//                connections.Clear();
//            }
//        }
//    }
//}