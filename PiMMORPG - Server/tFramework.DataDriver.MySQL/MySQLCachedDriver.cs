//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using tFramework.Data.Interfaces;
//using MySql.Data.MySqlClient;

//namespace tFramework.DataDriver.MySQL
//{
//    public class MySqlCachedDriver<TModel> : BaseCachedDriver<TModel, MySqlConnection>
//        where TModel : class, IModel, new()
//    {
//        public MySqlCachedDriver(string connectionString) : base(connectionString)
//        {
//            var builder = new MySqlConnectionStringBuilder(connectionString);
//            Database = builder.Database;
//            builder.Database = null;
//            base.ConnectionString = builder.ConnectionString;
//        }

//        protected override object GetIdsValue(uint[] ds)
//        {
//            return MySqlDriverHelper.GetIdsValue(ds);
//        }

//        protected override uint[] GetModelIDs(object value)
//        {
//            return MySqlDriverHelper.GetModelIDs(value);
//        }

//        protected override string GetTypeName(Type target)
//        {
//            return MySqlDriverHelper.GetTypeName(target);
//        }
//    }
//}
