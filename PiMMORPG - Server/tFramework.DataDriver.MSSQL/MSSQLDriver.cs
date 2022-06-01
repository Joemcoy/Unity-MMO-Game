using System;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

using tFramework.Data.Interfaces;

namespace tFramework.DataDriver.MSSQL
{
    public class MSSQLDriver<TModel> : BaseDriver<TModel, SqlConnection>
        where TModel : class, IModel, new()
    {
        public MSSQLDriver(string connectionString) : base(connectionString)
        {
        }

        protected override void PreConnection()
        {
            base.PreConnection();
            var builder = new SqlConnectionStringBuilder(ConnectionString);
            Database = builder.InitialCatalog;
            builder.InitialCatalog = "master";
            ConnectionString = builder.ConnectionString;

            Settings.OpenDefinitionChar = '[';
            Settings.CloseDefinitionChar = ']';
            Settings.CheckDatabaseQuery = $"SELECT COUNT(name) FROM sys.databases WHERE name = '{Database}'";
            Settings.CheckTableQuery = $"SELECT COUNT(TABLE_NAME) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_CATALOG = '{Database}' AND TABLE_NAME = '{TableName}'";
            Settings.CreateTableQuery = $"CREATE TABLE [{TableName}] ([ID] INT PRIMARY KEY IDENTITY(1,1), {{0}})";
            Settings.LastInsertIDQuery = "SELECT SCOPE_IDENTITY()";
            Settings.LimitSettings.Keyword = "TOP {0}";
            Settings.LimitSettings.Alignment = OperationAlignment.Left;
            Settings.OffsetSettings.Keyword = "OFFSET {0} ROWS";
            Settings.OffsetSettings.Alignment = OperationAlignment.Right;
            Settings.PaginationSettings.Keyword = "OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY";
            Settings.PaginationSettings.Alignment = OperationAlignment.Right;
        }

        protected override bool CheckColumnExists(string Column)
        {
            using (var connection = CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_CATALOG = '{Database}' AND TABLE_NAME = '{TableName}' AND COLUMN_NAME = '{Column}'";
                    return Convert.ToInt32(command.ExecuteScalar()) > 0;
                }
            }
        }

        protected override object GetIDsValue(int[] ds)
        {
            return MSSQLDriverHelper.GetIdsValue(ds);
        }

        protected override int[] GetModelIDs(object value)
        {
            return MSSQLDriverHelper.GetModelIDs(value);
        }

        protected override string GetTypeName(Type target)
        {
            return MSSQLDriverHelper.GetTypeName(target);
        }

        protected override object PrepareValue(Type type, object value, char separatorChar = ',')
        {
            var val = base.PrepareValue(type, value);

            if (val != null && val != DBNull.Value)
            {
                if (type == typeof(ushort))
                    val = Convert.ToInt16(val);
                else if (type == typeof(uint))
                    val = Convert.ToInt32(val);
                else if (type == typeof(ulong))
                    val = Convert.ToInt64(val);
            }
            return val;
        }
    }
}
