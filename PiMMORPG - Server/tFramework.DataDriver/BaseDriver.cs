using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.Common;
using System.Collections;
using System.Data.SqlTypes;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace tFramework.DataDriver
{
    using tFramework.Interfaces;
    using Enums;
    using Helper;
    using Interfaces;
    using EventArgs;
    using Factories;

    using Data.Interfaces;

    public abstract class BaseDriver<TModel, TConnection> : IDriver<TModel>, IDisposable
        where TModel : class, IModel, new()
        where TConnection : DbConnection, new()
    {
        //protected DbConnection Connection;

        List<TConnection> connections;
        ILogger logger;
        Type baseType;
        bool firstPass = false;
        string defConnString;

        protected IDriver Base { get { return this as IDriver; } }
        protected string Database { get; set; }
        public string ConnectionString { get; set; }
        public DriverSettings<TModel> Settings { get; set; }

        protected virtual string TableName
        {
            get
            {
                var tn = typeof(TModel).Name.ToLower();
                if (!tn.ToLower().EndsWith("s"))
                    tn += "s";
                return tn;
            }
        }

        Type IDriver.ModelType
        {
            get { return typeof(TModel); }
        }

        protected PropertyInfo[] Properties => typeof(TModel).GetProperties().Where(p => p.CanWrite && p.CanRead && !Settings.IsIgnored(p)).ToArray();
        protected string[] Columns => Properties.Select(p => Settings.GetColumn(p.Name)).ToArray();
        protected string[] NonIDColumns => Columns.Where(c => c != Settings.GetColumn("ID")).ToArray();

        protected abstract bool CheckColumnExists(string Column);
        protected virtual void PreConnection() { }
        protected virtual void PosConnection() { }
        protected virtual void OnCreateTable() { }

        protected virtual void OnLoad(TModel model) { }
        protected virtual void OnInsert(TModel model) { }
        protected virtual void OnUpdate(TModel model) { }
        protected virtual void OnDelete(TModel model) { }

        protected abstract string GetTypeName(Type target);
        protected abstract int[] GetModelIDs(object value);
        protected abstract object GetIDsValue(int[] ds);

        public BaseDriver(string connectionString)
        {
            baseType = GetType();
            connections = new List<TConnection>();
            logger = LoggerFactory.GetLogger(this);

            defConnString = connectionString;
            ConnectionString = connectionString;

            Settings = new DriverSettings<TModel>(this);
            PreConnection();

            if (Settings.AutoCreateDatabase || Settings.AutoCreateTable)
                CheckSchema();
            else
                firstPass = true;
            PosConnection();
        }

        protected TConnection PrepareConnection(bool useDefault = false)
        {
            var str = useDefault ? defConnString : ConnectionString;

            var connection = new TConnection();
            connections.Add(connection);
            connection.Disposed += (s, e) => connections.Remove(connection);
            connection.ConnectionString = str;

            return connection;
        }

        protected TConnection CreateConnection(bool useDefault = false)
        {
            var connection = PrepareConnection(useDefault);

            connection.Open();
            if (firstPass && connection.Database != Database)
                connection.ChangeDatabase(Database);

            return connection;
        }
        
        private void CheckSchema()
        {
            using (var connection = CreateConnection(false))            
            {
                var checkFlag = false;
                var command = connection.CreateCommand();

                if (Settings.AutoCreateDatabase && !string.IsNullOrEmpty(Settings.CheckDatabaseQuery))
                {
                    command.CommandText = Settings.CheckDatabaseQuery;

                    if (Convert.ToInt32(command.ExecuteScalar()) == 0)
                    {
                        logger.LogWarning($"Database {Database} not found, creating...");
                        checkFlag = true;

                        command.CommandText = $"CREATE DATABASE {Settings.OpenDefinitionChar}{Database}{Settings.CloseDefinitionChar}";
                        command.ExecuteNonQuery();
                    }
                    connection.ChangeDatabase(Database);
                }
                firstPass = true;
                connection.ChangeDatabase(Database);

                if (!checkFlag)
                    command.CommandText = Settings.CheckTableQuery;
                if (Settings.AutoCreateTable && (checkFlag || Convert.ToInt32(command.ExecuteScalar()) == 0))
                {
                    logger.LogWarning($"Table {TableName} not found, creating...");

                    checkFlag = true;
                    command.CommandText = string.Format(Settings.CreateTableQuery, GetCreateDefinitions());
                    command.ExecuteNonQuery();

                    OnCreateTable();
                }

                if (!checkFlag)
                {
                    var temp = new List<string>();
                    foreach (string column in Columns)
                    {
                        if (!(CheckColumnExists(column)))
                        {
                            logger.LogWarning($"Column {column} of table {TableName} not found, creating....");
                            temp.Add(column);
                        }
                    }

                    if (temp.Count > 0)
                    {
                        command.CommandText = $"ALTER TABLE {Settings.OpenDefinitionChar}{TableName}{Settings.CloseDefinitionChar} ADD {Settings.OpenDefinitionChar}{temp[0]}{Settings.CloseDefinitionChar} {GetTypeName(Properties.First(p => p.Name == Settings.GetColumn(temp[0], false)).PropertyType)};";
                        foreach (var column in temp.Skip(1))
                            command.CommandText += $"ALTER TABLE {Settings.OpenDefinitionChar}{TableName}{Settings.CloseDefinitionChar} ADD {Settings.OpenDefinitionChar}{column}{Settings.CloseDefinitionChar} {GetTypeName(Properties.First(p => p.Name == Settings.GetColumn(column, false)).PropertyType)};";
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private string GetCreateDefinitions()
        {
            string definition = string.Empty;

            foreach (var property in Properties)
            {
                if (property.Name != "ID")
                    definition += $"{Settings.OpenDefinitionChar}{Settings.GetColumn(property.Name)}{Settings.CloseDefinitionChar} {GetTypeName(property.PropertyType)},";
            }

            if (definition.EndsWith(","))
                definition = definition.Substring(0, definition.Length - 1);
            return definition;
        }

        public void AddModel(IModel model)
        {
            if (model is ISerialModel)
            {
                var smodel = (model as ISerialModel);
                if (smodel.Serial == Guid.Empty)
                    smodel.Serial = Guid.NewGuid();
            }
            TModel T = model as TModel;

            using (var connection = CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT COUNT({Settings.GetColumn("ID")}) FROM {Settings.OpenDefinitionChar}{TableName}{Settings.CloseDefinitionChar} WHERE {Settings.GetColumn("ID")} = {model.ID}";
                if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                    UpdateModel(model);
                else
                {                    

                    command.CommandText = $"INSERT INTO {Settings.OpenDefinitionChar}{TableName}{Settings.CloseDefinitionChar} ({{0}}) VALUES({{1}});" + Settings.LastInsertIDQuery;
                    foreach (PropertyInfo property in Properties)
                    {
                        if (property.Name != "ID")
                        {
                            if (Settings.IsMapped(property.Name))
                            {
                                var map = Settings.GetMappedDriver(property.Name);
                                if (map.UpdateOrRemove)
                                {
                                    var value = property.GetValue(model, null);

                                    if (value != null)
                                    {
                                        if (property.PropertyType.IsArray)
                                            foreach (object m in value as Array)
                                                map.Driver.AddModel(m as IModel);
                                        else
                                            map.Driver.AddModel(value as IModel);
                                    }
                                }
                            }

                            var parameter = command.CreateParameter();
                            if (PrepareParameter(parameter, property, T))
                                command.Parameters.Add(parameter);
                        }
                    }

                    var pArr = command.Parameters.OfType<DbParameter>().ToArray();
                    var names = string.Join(",", pArr.Select(p => Settings.OpenDefinitionChar + Settings.GetColumn(p.ParameterName.Substring(2)) + Settings.CloseDefinitionChar).ToArray());

                    command.CommandText = string.Format(command.CommandText, names, string.Join(",", pArr.Select(p => p.ParameterName).ToArray()));
                    model.ID = Convert.ToInt32(command.ExecuteScalar());
                    OnInsert(T);
                }
            }
        }

        public async Task AddModelAsync(IModel model)
        {
            if (model is ISerialModel)
            {
                var smodel = (model as ISerialModel);
                if (smodel.Serial == Guid.Empty)
                    smodel.Serial = Guid.NewGuid();
            }
            TModel T = model as TModel;

            using (var connection = CreateConnection())
            
            {
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT COUNT({Settings.GetColumn("ID")}) FROM {Settings.OpenDefinitionChar}{TableName}{Settings.CloseDefinitionChar} WHERE {Settings.GetColumn("ID")} = {model.ID}";
                if (Convert.ToInt32(await command.ExecuteScalarAsync()) > 0)
                    await UpdateModelAsync(model);
                else
                {                    
                    command.CommandText = "INSERT INTO {Settings.OpenDefinitionChar}{TableName}{Settings.CloseDefinitionChar} ({{0}}) VALUES {{1}})" + Settings.LastInsertIDQuery;
                    foreach (PropertyInfo property in Properties)
                    {
                        if (property.Name != "ID")
                        {
                            if (Settings.IsMapped(property.Name))
                            {
                                var map = Settings.GetMappedDriver(property.Name);
                                if (map.UpdateOrRemove)
                                {
                                    var value = property.GetValue(model, null);

                                    if (value != null)
                                    {
                                        if (property.PropertyType.IsArray)
                                            foreach (object m in value as Array)
                                                await map.Driver.AddModelAsync(m as IModel);
                                        else
                                            await map.Driver.AddModelAsync(value as IModel);
                                    }
                                }
                            }
                            var parameter = command.CreateParameter();
                            if (PrepareParameter(parameter, property, T))
                                command.Parameters.Add(parameter);
                        }
                    }

                    var pArr = command.Parameters.OfType<DbParameter>().ToArray();
                    var names = string.Join(",", pArr.Select(p => Settings.OpenDefinitionChar + Settings.GetColumn(p.ParameterName.Substring(2)) + Settings.CloseDefinitionChar).ToArray());

                    command.CommandText = string.Format(command.CommandText, names, string.Join(",", pArr.Select(p => p.ParameterName).ToArray()));
                    model.ID = Convert.ToInt32(await command.ExecuteScalarAsync());
                    OnInsert(T);
                }
            }
        }

        public void UpdateModel(IModel model)
        {
            using (var connection = CreateConnection())
            {
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT COUNT({Settings.GetColumn("ID")}) FROM {Settings.OpenDefinitionChar}{TableName}{Settings.CloseDefinitionChar} WHERE {Settings.GetColumn("ID")} = {model.ID}";
                if (Convert.ToInt32(command.ExecuteScalar()) == 0)
                    AddModel(model);
                else
                {
                    OnUpdate(model as TModel);

                    command.CommandText = $"UPDATE {Settings.OpenDefinitionChar}{TableName}{Settings.CloseDefinitionChar} SET {{0}} WHERE {Settings.GetColumn("ID")} = @_ID";
                    foreach (PropertyInfo property in Properties)
                    {
                        //string.Join(",", NonIDColumns.Select(c => string.Format($"{Settings.OpenDefinitionChar}{c}{Settings.CloseDefinitionChar} = @_{c}")).ToArray())
                        var parameter = command.CreateParameter();
                        if(PrepareParameter(parameter, property, model as TModel))
                            command.Parameters.Add(parameter);

                        if (Settings.IsMapped(property.Name))
                        {
                            var map = Settings.GetMappedDriver(property.Name);

                            if (map.UpdateOrRemove)
                            {
                                var value = property.GetValue(model, null);

                                if (value != null)
                                    map.Driver.UpdateModel(value as IModel);
                            }
                        }
                    }

                    var pArr = command.Parameters.OfType<DbParameter>().Where(p => p.ParameterName != "@_ID").Select(p => $"{Settings.OpenDefinitionChar}{p.ParameterName.Substring(2)}{Settings.CloseDefinitionChar} = {p.ParameterName}").ToArray();
                    command.CommandText = string.Format(command.CommandText, string.Join(",", pArr));

                    if (!command.Parameters.Contains("@_ID"))
                    {
                        var idp = command.CreateParameter();
                        idp.ParameterName = "@_ID";
                        idp.Value = model.ID;
                        command.Parameters.Add(idp);
                    }
                    command.ExecuteNonQuery();
                }
            }
        }

        public async Task UpdateModelAsync(IModel model)
        {
            using (var connection = CreateConnection())            
            {
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT COUNT({Settings.GetColumn("ID")}) FROM {Settings.OpenDefinitionChar}{TableName}{Settings.CloseDefinitionChar} WHERE {Settings.GetColumn("ID")} = {model.ID}";
                if (Convert.ToInt32(await command.ExecuteScalarAsync()) == 0)
                    await AddModelAsync(model);
                else
                {
                    command.CommandText = $"UPDATE {Settings.OpenDefinitionChar}{TableName}{Settings.CloseDefinitionChar} SET {{0}} WHERE {Settings.GetColumn("ID")} = @_ID";
                    foreach (PropertyInfo property in Properties)
                    {
                        //string.Join(",", NonIDColumns.Select(c => string.Format($"{Settings.OpenDefinitionChar}{c}{Settings.CloseDefinitionChar} = @_{c}")).ToArray())
                        var parameter = command.CreateParameter();
                        if (PrepareParameter(parameter, property, model as TModel))
                            command.Parameters.Add(parameter);

                        if (Settings.IsMapped(property.Name))
                        {
                            var map = Settings.GetMappedDriver(property.Name);

                            if (map.UpdateOrRemove)
                            {
                                var value = property.GetValue(model, null);

                                if (value != null)
                                    await map.Driver.UpdateModelAsync(value as IModel);
                            }
                        }
                    }

                    var pArr = command.Parameters.OfType<DbParameter>().Where(p => p.ParameterName != "@_ID").Select(p => $"{Settings.OpenDefinitionChar}{p.ParameterName.Substring(2)}{Settings.CloseDefinitionChar} = {p.ParameterName}").ToArray();
                    command.CommandText = string.Format(command.CommandText, string.Join(",", pArr));

                    if (!command.Parameters.Contains("@_ID"))
                    {
                        var idp = command.CreateParameter();
                        idp.ParameterName = "@_ID";
                        idp.Value = model.ID;
                        command.Parameters.Add(idp);
                    }
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public void RemoveModel(IModel model)
        {
            ThreadingHelper.RunSync(RemoveModelAsync(model));
        }

        public async Task RemoveModelAsync(IModel model)
        {
            using (var connection = CreateConnection())            
            {
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT COUNT(ID) FROM {Settings.OpenDefinitionChar}{TableName}{Settings.CloseDefinitionChar} WHERE {Settings.GetColumn("ID")} = {model.ID}";
                if (Convert.ToInt32(await command.ExecuteScalarAsync()) > 0)
                {
                    OnDelete(model as TModel);

                    command.CommandText = $"DELETE FROM {Settings.OpenDefinitionChar}{TableName}{Settings.CloseDefinitionChar} WHERE {Settings.GetColumn("ID")} = @ID";

                    var param = command.CreateParameter();
                    param.ParameterName = "@ID";
                    param.Value = model.ID;
                    command.Parameters.Add(param);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public void RemoveModel(QueryBuilder<TModel> Builder = null)
        {
            using (var connection = CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT {{0}} * FROM {Settings.OpenDefinitionChar}{TableName}{Settings.CloseDefinitionChar} {{1}}";
                    PrepareFromBuilder(command, Builder);

                    var toRemove = new List<IModel>();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                var model = Fill(reader);
                                toRemove.Add(model);
                                OnDelete(model);
                            }
                        }
                    }

                    foreach (var model in toRemove)
                    {
                        command.CommandText = $"DELETE FROM {Settings.OpenDefinitionChar}{TableName}{Settings.CloseDefinitionChar} WHERE {Settings.GetColumn("ID")} = @ID";

                        var param = command.CreateParameter();
                        param.ParameterName = "@ID";
                        param.Value = model.ID;
                        command.Parameters.Add(param);

                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public async Task RemoveModelAsync(QueryBuilder<TModel> Builder = null)
        {
            using (var connection = CreateConnection())            
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT {{0}} * FROM {Settings.OpenDefinitionChar}{TableName}{Settings.CloseDefinitionChar} {{1}}";
                    PrepareFromBuilder(command, Builder);

                    var toRemove = new List<IModel>();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                var model = await FillAsync(reader);
                                toRemove.Add(model);
                                OnDelete(model);
                            }
                        }
                    }

                    foreach (var model in toRemove)
                    {
                        command.CommandText = $"DELETE FROM {Settings.OpenDefinitionChar}{TableName}{Settings.CloseDefinitionChar} WHERE {Settings.GetColumn("ID")} = @ID";

                        var param = command.CreateParameter();
                        param.ParameterName = "@ID";
                        param.Value = model.ID;
                        command.Parameters.Add(param);

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
        }

        public void AddModel(TModel model)
        {
            Base.AddModel(model);
        }

        public async Task AddModelAsync(TModel model)
        {
            await Base.AddModelAsync(model);
        }

        public void UpdateModel(TModel model)
        {
            Base.UpdateModel(model);
        }

        public async Task UpdateModelAsync(TModel model)
        {
            await Base.UpdateModelAsync(model);
        }

        public void RemoveModel(TModel model)
        {
            Base.RemoveModel(model);
        }

        public async Task RemoveModelAsync(TModel model)
        {
            await Base.RemoveModelAsync(model);
        }

        public QueryBuilder<TModel> CreateBuilder()
        {
            return new QueryBuilder<TModel>(this);
        }

        public TModel GetModel(int ID)
        {
            return Base.GetModel(ID) as TModel;
        }

        public async Task<TModel> GetModelAsync(int id)
        {
            return await Base.GetModelAsync(id) as TModel;
        }

        public TModel GetModel(QueryBuilder<TModel> Builder = null)
        {
            TModel model = null;

            using (var connection = CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT {{0}} * FROM {Settings.OpenDefinitionChar}{TableName}{Settings.CloseDefinitionChar} {{1}}";
                    PrepareFromBuilder(command, Builder);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            model = Fill(reader);
                        }
                    }
                }
            }
            return model;
        }

        public async Task<TModel> GetModelAsync(QueryBuilder<TModel> Builder = null)
        {
            TModel model = null;

            using (var connection = CreateConnection())            
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT {{0}} * FROM {Settings.OpenDefinitionChar}{TableName}{Settings.CloseDefinitionChar} {{1}}";
                    PrepareFromBuilder(command, Builder);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            model = await FillAsync(reader);
                        }
                    }
                }
            }
            return model;
        }

        public TModel[] GetModels(params int[] ids)
        {
            if (ids == null)
                return new TModel[0];

            var arr = new TModel[ids.Length];
            for (int i = 0; i < ids.Length; i++)
                arr[i] = GetModel(ids[i]);
            return arr.OfType<TModel>().ToArray();
        }

        public async Task<TModel[]> GetModelsAsync(params int[] ids)
        {
            if (ids == null)
                return new TModel[0];

            var arr = new TModel[ids.Length];
            for (int i = 0; i < ids.Length; i++)
                arr[i] = await GetModelAsync(ids[i]);
            return arr.OfType<TModel>().ToArray();
        }

        public TModel[] GetModels(QueryBuilder<TModel> Builder = null)
        {
            if (Builder == null)
                Builder = CreateBuilder();

            TModel[] models = null;
            using (var connection = CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT {{0}} * FROM {Settings.OpenDefinitionChar}{TableName}{Settings.CloseDefinitionChar} {{1}}";
                    PrepareFromBuilder(command, Builder);

                    var temp = new List<TModel>();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var Model = Fill(reader);

                            temp.Add(Model);
                        }
                    }

                    models = temp.ToArray();
                }
            }

            return models;
        }

        public async Task<TModel[]> GetModelsAsync(QueryBuilder<TModel> Builder = null)
        {
            if (Builder == null)
                Builder = CreateBuilder();

            TModel[] models = null;
            using (var connection = CreateConnection())            
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT {{0}} * FROM {Settings.OpenDefinitionChar}{TableName}{Settings.CloseDefinitionChar} {{1}}";
                    PrepareFromBuilder(command, Builder);

                    var temp = new List<TModel>();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var Model = await FillAsync(reader);

                            temp.Add(Model);
                        }
                    }

                    models = temp.ToArray();
                }
            }

            return models;
        }

        public bool HasModel(out TModel model, QueryBuilder<TModel> Builder = null)
        {
            if (Builder == null)
                Builder = CreateBuilder();
            
            using (var connection = CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT {{0}} * FROM {Settings.OpenDefinitionChar}{TableName}{Settings.CloseDefinitionChar} {{1}}";
                    PrepareFromBuilder(command, Builder);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows && reader.Read())
                        {
                            model = Fill(reader);
                            return true;
                        }
                        else
                        {
                            model = null;
                            return false;
                        }
                    }
                }
            }
        }

        public bool HasModel(QueryBuilder<TModel> Builder = null)
        {
            TModel model;
            return HasModel(out model, Builder);
        }

        public async Task<Tuple<bool, TModel>> HasModelAsync(QueryBuilder<TModel> Builder = null)
        {
            if (Builder == null)
                Builder = CreateBuilder();

            bool result = false;
            TModel model;
            using (var connection = CreateConnection())            
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT {{0}} * FROM {Settings.OpenDefinitionChar}{TableName}{Settings.CloseDefinitionChar} {{1}}";
                    PrepareFromBuilder(command, Builder);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows && reader.Read())
                        {
                            model = await FillAsync(reader);

                            result = true;
                        }
                        else
                        {
                            model = null;
                            result = false;
                        }
                    }
                }
            }

            return new Tuple<bool, TModel>(result, model);
        }

        public int Count(QueryBuilder<TModel> Builder = null)
        {
            if (Builder == null)
                Builder = CreateBuilder();
            int result = -1;

            using (var connection = CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT {{0}} COUNT({Settings.GetColumn("ID")}) FROM {Settings.OpenDefinitionChar}{TableName}{Settings.CloseDefinitionChar} {{1}}";
                    PrepareFromBuilder(command, Builder);

                    result = Convert.ToInt32(command.ExecuteScalar());
                }
            }

            return result;
        }

        public async Task<int> CountAsync(QueryBuilder<TModel> Builder = null)
        {
            if (Builder == null)
                Builder = CreateBuilder();
            int result = -1;

            using (var connection = CreateConnection())            
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT {{0}} COUNT({Settings.GetColumn("ID")}) FROM {Settings.OpenDefinitionChar}{TableName}{Settings.CloseDefinitionChar} {{1}}";
                    PrepareFromBuilder(command, Builder);

                    result = Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }

            return result;
        }

        IModel IDriver.GetModel(int ID)
        {
            TModel model = null;
            using (var connection = CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT * FROM {Settings.OpenDefinitionChar}{TableName}{Settings.CloseDefinitionChar} WHERE {Settings.GetColumn("ID")} = @ID";

                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "@ID";
                    parameter.Value = PrepareValue(typeof(int), ID, Settings.GetSeparatorChar("ID"));
                    command.Parameters.Add(parameter);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows && reader.Read())
                        {
                            model = Fill(reader);
                        }
                        else model = null;
                    }
                }
            }

            return model;
        }

        async Task<IModel> IDriver.GetModelAsync(int ID)
        {
            TModel model = null;
            using (var connection = CreateConnection())            
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT * FROM {Settings.OpenDefinitionChar}{TableName}{Settings.CloseDefinitionChar} WHERE {Settings.GetColumn("ID")} = @ID";

                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "@ID";
                    parameter.Value = PrepareValue(typeof(int), ID, Settings.GetSeparatorChar("ID"));
                    command.Parameters.Add(parameter);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (reader.HasRows && await reader.ReadAsync())
                        {
                            model = await FillAsync(reader);
                        }
                        else model = null;
                    }
                }
            }
            
            return model;
        }

        IModel[] IDriver.GetModels(params int[] ids)
        {
            if (ids == null)
                return new TModel[0];

            IModel[] models = null;
            using (var connection = CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT * FROM {Settings.OpenDefinitionChar}{TableName}{Settings.CloseDefinitionChar}";
                    for (int i = 0; i < ids.Length; i++)
                    {
                        if (i == 0)
                            command.CommandText += " WHERE ";
                        else
                            command.CommandText += " OR ";
                        command.CommandText += string.Format("{0} = @ID{1}", Settings.GetColumn("ID"), i);

                        var parameter = command.CreateParameter();
                        parameter.ParameterName = string.Format("@ID{0}", i);
                        parameter.Value = ids[i];

                        command.Parameters.Add(parameter);
                    }

                    var temp = new List<TModel>();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.HasRows && reader.Read())
                        {
                            var Model = Fill(reader);
                            temp.Add(Model);
                        }
                    }

                    models = temp.ToArray();
                }
            }

            return models;
        }

        async Task<IModel[]> IDriver.GetModelsAsync(params int[] ids)
        {
            if (ids == null)
                return new TModel[0];

            IModel[] models = null;
            using (var connection = CreateConnection())            
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT * FROM {Settings.OpenDefinitionChar}{TableName}{Settings.CloseDefinitionChar}";
                    for (int i = 0; i < ids.Length; i++)
                    {
                        if (i == 0)
                            command.CommandText += " WHERE ";
                        else
                            command.CommandText += " OR ";
                        command.CommandText += string.Format("{0} = @ID{1}", Settings.GetColumn("ID"), i);

                        var parameter = command.CreateParameter();
                        parameter.ParameterName = string.Format("@ID{0}", i);
                        parameter.Value = ids[i];

                        command.Parameters.Add(parameter);
                    }

                    var temp = new List<TModel>();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.HasRows && await reader.ReadAsync())
                        {
                            var Model = await FillAsync(reader);
                            temp.Add(Model);
                        }
                    }

                    models = temp.ToArray();
                }
            }
            
            return models;
        }

        private bool PrepareParameter(DbParameter parameter, PropertyInfo property, TModel model)
        {
            var value = property.GetValue(model);
            value = PrepareValue(property.PropertyType, value, Settings.GetSeparatorChar(property.Name));

            if (value == DBNull.Value || value == null) return false;
            parameter.Value = value;
            parameter.ParameterName = "@_" + Settings.GetColumn(property.Name);
            return true;
        }

        protected virtual object PrepareValue(Type type, object value, char separatorChar = ',')
        {
            object def = null;
            if (value != null && (!type.IsClass || type.IsValueType))
                def = Activator.CreateInstance(type);

            if (value != def)
            {
                if (value is IModel)
                    value = (value as IModel).ID;
                else if (type.IsArray && typeof(IModel).IsAssignableFrom(type.GetElementType()))
                {
                    var temp = string.Empty;
                    var array = value as Array;

                    for (int i = 0; i < array.Length; i++)
                    {
                        temp += (array.GetValue(i) as IModel).ID;
                        if (i < array.Length - 1)
                            temp += separatorChar;
                    }
                    value = temp;
                }
                else if (type.IsArray)
                {
                    var arr = value as Array;
                    var temp = string.Empty;

                    for (int i = 0; i < arr.Length; i++)
                    {
                        temp += Convert.ToString(arr.GetValue(i));
                        if (i < arr.Length - 1)
                            temp += separatorChar;
                    }
                    value = temp;
                }
                else if (value is Guid)
                    value = ((Guid)value).ToString();
                else if (value is DateTime && ((DateTime)value < SqlDateTime.MinValue.Value || (DateTime)value > SqlDateTime.MaxValue.Value))
                    value = DBNull.Value;
            }
            else
                value = DBNull.Value;
            return value;
        }

        private object PrepareValue(object value, DbDataReader reader, Type type, string name, int index)
        {
            value = reader.GetValue(index);

            if (Settings.IsMapped(name))
            {
                var driver = Settings.GetMappedDriver(name).Driver;
                if (typeof(IModel).IsAssignableFrom(type))
                {
                    var id = Convert.ToInt32(value);
                    value = driver.GetModel(id);
                }
                else if (type.IsArray || typeof(IList<>).IsAssignableFrom(type))
                {
                    var ids = GetModelIDs(value);
                    var values = driver.GetModels(ids);

                    if (type.IsArray)
                        value = values.Select(v => Convert.ChangeType(v, type.GetElementType())).ToArray();
                    else
                    {
                        var list = Activator.CreateInstance(type) as IList;
                        foreach (var item in values)
                            list.Add(Convert.ChangeType(item, type.GetGenericArguments()[0]));
                        value = list;
                    }
                }
            }
            else if (typeof(IModel).IsAssignableFrom(type))
            {
                var tmodel = Activator.CreateInstance(type) as IModel;
                tmodel.ID = Convert.ToInt32(value);
                value = tmodel;
            }
            else if (typeof(Guid) == type)
                value = Guid.Parse(Convert.ToString(value));
            else if (type.IsEnum)
                value = Enum.ToObject(type, value);
            else if(type.IsArray)
            {
                var str = Convert.ToString(value);
                var map = str.Split(Settings.GetSeparatorChar(name));
                var atype = type.GetElementType();

                var arr = Array.CreateInstance(atype, map.Length);
                if (map.Length > 0)
                {
                    for(int i = 0; i < map.Length; i++)
                    {
                        try
                        {
                            if (type.IsEnum)
                                arr.SetValue(Enum.ToObject(atype, map[i]), i);
                            else if (type == typeof(Guid))
                                arr.SetValue(Guid.Parse(map[i]), i);
                            else
                                arr.SetValue(Convert.ChangeType(map[i], atype), i);
                        }
                        catch(FormatException)
                        {
                            arr.SetValue(Activator.CreateInstance(atype), i);
                        }
                    }
                }
                value = arr;
            }

            if (Nullable.GetUnderlyingType(type) != null)
                type = new NullableConverter(type).UnderlyingType;

            return value;
        }

        private TModel Fill(DbDataReader reader)
        {
            var model = new TModel();

            foreach (var property in Properties)
            {
                var type = property.PropertyType;
                var name = property.Name;
                var index = reader.GetOrdinal(Settings.GetColumn(name));

                name = Settings.GetColumn(name, false);
                object value = null;

                if (Nullable.GetUnderlyingType(type) != null)
                    value = Activator.CreateInstance(type, null);

                if (!reader.IsDBNull(index))
                    value = PrepareValue(value, reader, type, name, index);
                property.SetValue(model, value == null ? null : Convert.ChangeType(value, type));
            }

            OnLoad(model);
            return model;
        }

        private async Task<TModel> FillAsync(DbDataReader reader)
        {
            var model = new TModel();

            foreach (var property in Properties)
            {
                var type = property.PropertyType;
                var name = property.Name;
                var index = reader.GetOrdinal(Settings.GetColumn(name));

                name = Settings.GetColumn(name, false);
                object value = null;

                if (Nullable.GetUnderlyingType(type) != null)
                    value = Activator.CreateInstance(type, null);

                if (!(await reader.IsDBNullAsync(index)))
                    value = PrepareValue(value, reader, type, name, index);
                property.SetValue(model, value == null ? null : Convert.ChangeType(value, type));
            }

            OnLoad(model);
            return model;
        }

        protected void PrepareFromBuilder(DbCommand command, QueryBuilder<TModel> builder)
        {
            if (builder != null)
            {
                command.CommandText = string.Format(command.CommandText, builder.LeftPartial, builder.RightPartial);
                foreach (var name in builder.Parameters.Keys)
                {
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = name;

                    var val = builder.Parameters[name];
                    var sep = Settings.GetSeparatorChar(name.StartsWith("@") ? name.Substring(2) : name);
                    parameter.Value = PrepareValue(val == null ? null : val.GetType(), val, sep);

                    command.Parameters.Add(parameter);
                }
            }
            else
                command.CommandText = string.Format(command.CommandText, string.Empty, string.Empty);
        }

        bool disposed = false;
        public void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
                /*foreach (IDriver driver in drivers.Values)
                    driver.Dispose();
                drivers.Clear();*/

                if (!Settings.MaintainConnection)
                    Settings.Dispose();

            }
        }
    }
}