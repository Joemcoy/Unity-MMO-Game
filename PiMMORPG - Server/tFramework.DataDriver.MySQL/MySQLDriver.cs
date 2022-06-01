using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using tFramework.Data.Interfaces;
using MySql.Data.MySqlClient;

namespace tFramework.DataDriver.MySQL
{
    public class MySqlDriver<TModel> : BaseDriver<TModel, MySqlConnection>
        where TModel : class, IModel, new()
    {
        public MySqlDriver(string connectionString) : base(connectionString)
        {
            
        }

        protected override void PreConnection()
        {
            var builder = new MySqlConnectionStringBuilder(ConnectionString);
            Database = builder.Database;
            builder.Database = null;
            ConnectionString = builder.ConnectionString;

            Settings.CheckDatabaseQuery = $"SELECT COUNT(SCHEMA_NAME) FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = '{Database}'";
            Settings.CheckTableQuery = $"SELECT COUNT(TABLE_NAME) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '{Database}' AND TABLE_NAME = '{TableName}'";
            Settings.CreateTableQuery = $"CREATE TABLE `{TableName}` (`ID` INT UNSIGNED PRIMARY KEY AUTO_INCREMENT, {{0}})";
            Settings.LastInsertIDQuery = "SELECT LAST_INSERT_ID()";
        }

        protected override bool CheckColumnExists(string Column)
        {
            using (var connection = CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '{Database}' AND TABLE_NAME = '{TableName}' AND COLUMN_NAME = '{Column}'";
                    return Convert.ToInt32(command.ExecuteScalar()) > 0;
                }
            }
        }

        protected override object GetIDsValue(int[] ds)
        {
            return MySqlDriverHelper.GetIdsValue(ds);
        }

        protected override int[] GetModelIDs(object value)
        {
            return MySqlDriverHelper.GetModelIDs(value);
        }

        protected override string GetTypeName(Type target)
        {
            return MySqlDriverHelper.GetTypeName(target);
        }
    }
}
