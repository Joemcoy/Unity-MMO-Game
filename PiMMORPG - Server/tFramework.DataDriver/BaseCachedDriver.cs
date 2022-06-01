//using System;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Data.Common;
//using System.Collections;
//using System.Collections.Generic;

//namespace tFramework.DataDriver
//{
//    using tFramework.Interfaces;
//    using Enums;
//    using Helper;
//    using Interfaces;
//    using EventArgs;
//    using Factories;

//    using Data.Interfaces;
//    using System.Reflection;
//    using tFramework.Enums;

//    public abstract class BaseCachedDriver<TModel, TConnection> : IUpdater, IComponent, ICachedDriver
//        where TModel : class, IModel, new()
//        where TConnection : DbConnection, new()
//    {
//        protected DbConnection Connection;
//        static object _syncLock = new object();
//        List<TModel> _models = new List<TModel>();
//        static BaseCachedDriver<TModel, TConnection> _instance;

//        List<ICachedDriver> _drivers = new List<ICachedDriver>();
//        static ICachedDriver Driver { get { return (_instance as ICachedDriver); } }
//        ILogger _logger;

//        public static event EventHandler<CachedDriverRefreshEventArgs<TModel>> Refreshed;
//        protected string Database { get; set; }
//        protected string ConnectionString { get; set; }

//        protected virtual string TableName
//        {
//            get { return typeof(TModel).Name.ToLower() + "s"; }
//        }

//        Type ICachedDriver.ModelType
//        {
//            get { return typeof(TModel); }
//        }

//        protected PropertyInfo[] Properties
//        {
//            get { return typeof(TModel).GetProperties().Where(p => p.CanWrite && p.CanRead).ToArray(); }
//        }

//        protected string[] Columns
//        {
//            get { return Properties.Select(p => p.Name).ToArray(); }
//        }

//        protected string[] NonIdColumns
//        {
//            get { return Columns.Where(c => c != "Id").ToArray(); }
//        }

//        protected virtual void PreConnection() { }
//        protected virtual void PosConnection() { }
//        protected virtual void OnCreateTable() { }

//        protected virtual void OnInsert(TModel model) { }
//        protected virtual void OnUpdate(TModel model) { }
//        protected virtual void OnDelete(TModel model) { }

//        protected abstract string GetTypeName(Type target);
//        protected abstract uint[] GetModelIDs(object value);
//        protected abstract object GetIdsValue(uint[] ds);

//        public BaseCachedDriver(string connectionString)
//        {
//            this.ConnectionString = connectionString;
//            _logger = LoggerFactory.GetLogger(this);
//            _instance = ComponentFactory.RegisterComponent(GetType(), this) as BaseCachedDriver<TModel, TConnection>;
//        }

//        public bool Enable()
//        {
//            try
//            {
//                lock (_syncLock)
//                {
//                    if (Connection != null && Connection.State != ConnectionState.Closed)
//                        return true;
//                    else
//                    {
//                        Connection = new TConnection();
//                        PreConnection();
//                        Connection.ConnectionString = ConnectionString;

//                        Connection.Open();
//                        CheckSchema();
//                        PosConnection();

//                        LoadModels();
//                        _logger.LogInfo($"Loaded {_models.Count} {typeof(TModel).Name} models!");

//                        ThreadFactory.Start(this);
//                        return true;
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogFatal(ex);
//                return false;
//            }
//        }

//        public bool Disable()
//        {
//            try
//            {
//                lock (_syncLock)
//                {
//                    ThreadFactory.Stop(this);
//                    Connection.Close();
//                    return true;
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogFatal(ex);
//                return false;
//            }
//        }

//        public static void AddModel(TModel model)
//        {
//            Driver.InternalAddModel(model);
//            Refreshed.FireEvent(new CachedDriverRefreshEventArgs<TModel>(Driver, DriverOperation.Insert, model), Driver);
//        }

//        void ICachedDriver.InternalAddModel(IModel model)
//        {
//            lock (_syncLock)
//            {
//                TModel T = model as TModel;
//                OnInsert(T);

//                var command = Connection.CreateCommand();
//                command.CommandText = $"SELECT COUNT(Id) FROM {TableName} WHERE Id = {model.ID}";
//                if (Convert.ToInt32(command.ExecuteScalar()) > 0)
//                    (this as ICachedDriver).InternalUpdateModel(model);
//                else
//                {
//                    command.CommandText = $"INSERT INTO {TableName} ({string.Join(",", NonIdColumns.Select(c => $"`{c}`").ToArray())}) VALUES({string.Join(",", NonIdColumns.Select(c => "@_" + c).ToArray())})";
//                    command.CommandText += ";SELECT LAST_INSERT_Id()";
//                    foreach (PropertyInfo property in Properties)
//                    {
//                        if (property.Name != "Id")
//                        {
//                            var parameter = command.CreateParameter();
//                            PrepareParameter(parameter, property, T);
//                            command.Parameters.Add(parameter);
//                        }
//                    }
//                    T.ID = Convert.ToUInt32(command.ExecuteScalar());
//                }

//                _models.Add(T);
//            }
//        }


//        void ICachedDriver.InternalUpdateModel(IModel model)
//        {
//            OnUpdate(model as TModel);

//            var command = Connection.CreateCommand();
//            command.CommandText = $"SELECT COUNT(Id) FROM {TableName} WHERE Id = {model.ID}";
//            if (Convert.ToInt32(command.ExecuteScalar()) == 0)
//                (this as ICachedDriver).InternalAddModel(model);
//            else
//            {
//                command.CommandText = $"UPDATE {TableName} SET {string.Join(",", NonIdColumns.Select(c => "@_" + c).ToArray())} WHERE Id = @_Id";
//                foreach (PropertyInfo property in Properties)
//                {
//                    var parameter = command.CreateParameter();
//                    PrepareParameter(parameter, property, model as TModel);
//                    command.Parameters.Add(parameter);
//                }
//                command.ExecuteNonQuery();
//            }
//        }

//        public static void UpdateModel(TModel model)
//        {
//            Driver.InternalUpdateModel(model);
//            Refreshed.FireEvent(new CachedDriverRefreshEventArgs<TModel>(Driver, DriverOperation.Update, model), Driver);
//        }

//        void ICachedDriver.InternalRemoveModel(IModel model)
//        {
//            OnDelete(model as TModel);

//            var command = Connection.CreateCommand();
//            command.CommandText = $"SELECT COUNT(Id) FROM {TableName} WHERE Id = {model.ID}";
//            if (Convert.ToInt32(command.ExecuteScalar()) > 0)
//            {
//                command.CommandText = $"DELETE FROM {TableName} WHERE Id = @Id";

//                var param = command.CreateParameter();
//                param.ParameterName = "@Id";
//                param.Value = model.ID;
//                command.Parameters.Add(param);

//                command.ExecuteNonQuery();
//            }
//        }

//        public static void RemoveModel(TModel model)
//        {
//            Driver.InternalRemoveModel(model);
//            Refreshed.FireEvent(new CachedDriverRefreshEventArgs<TModel>(Driver, DriverOperation.Delete, model), Driver);
//        }

//        private void PrepareParameter(DbParameter parameter, PropertyInfo property, TModel model)
//        {
//            var value = property.GetValue(model);

//            if (value != null)
//            {
//                if (value is IModel)
//                    value = (value as IModel).ID;
//                else if (value.GetType().IsArray && typeof(IModel).IsAssignableFrom(value.GetType().GetElementType()))
//                {
//                    var temp = string.Empty;
//                    var array = value as Array;

//                    for (int i = 0; i < array.Length; i++)
//                    {
//                        temp += (array.GetValue(i) as IModel).ID;
//                        if (i < array.Length - 1)
//                            temp += ";";
//                    }
//                    value = temp;
//                }
//            }

//            parameter.ParameterName = "@_" + property.Name;
//            parameter.Value = value;
//        }

//        private void CheckSchema()
//        {
//            var checkFlag = false;
//            var command = Connection.CreateCommand();
//            command.CommandText = $"SELECT COUNT(SCHEMA_NAME) FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = '{Database}'";

//            if (Convert.ToInt32(command.ExecuteScalar()) == 0)
//            {
//                _logger.LogWarning($"Database {Database} not found, creating...");
//                checkFlag = true;

//                command.CommandText = $"CREATE DATABASE {Database}";
//                command.ExecuteNonQuery();
//            }
//            Connection.ChangeDatabase(Database);

//            if (!checkFlag)
//                command.CommandText = $"SELECT COUNT(TABLE_NAME) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = '{Database}' AND TABLE_NAME = '{TableName}'";
//            if (checkFlag || Convert.ToInt32(command.ExecuteScalar()) == 0)
//            {
//                _logger.LogWarning($"Table {TableName} not found, creating...");

//                checkFlag = true;
//                command.CommandText = $"CREATE TABLE {TableName} (Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY, {GetCreateDefinitions()})";
//                command.ExecuteNonQuery();

//                OnCreateTable();
//            }

//            if (!checkFlag)
//            {
//                foreach (string column in Columns)
//                {
//                    command.CommandText = $"SELECT COUNT(COLUMN_NAME) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '{Database}' AND TABLE_NAME = '{TableName}' AND COLUMN_NAME = '{column}'";
//                    if (Convert.ToInt32(command.ExecuteScalar()) == 0)
//                    {
//                        _logger.LogWarning($"Column {column} of table {TableName} not found, creating....");
//                        command.CommandText = $"ALTER TABLE {TableName} ADD `{column}` {GetTypeName(Properties.First(p => p.Name == column).PropertyType)}";
//                        command.ExecuteNonQuery();
//                    }
//                }
//            }
//        }

//        private string GetCreateDefinitions()
//        {
//            string definition = string.Empty;

//            foreach (var property in Properties)
//            {
//                if (property.Name != "Id")
//                    definition += $"`{property.Name}` {GetTypeName(property.PropertyType)},";
//            }

//            if (definition.EndsWith(","))
//                definition = definition.Substring(0, definition.Length - 1);
//            return definition;
//        }

//        private void LoadModels()
//        {
//            lock (_syncLock)
//            {
//                _models.Clear();

//                var command = Connection.CreateCommand();
//                command.CommandText = $"SELECT {string.Join(",", Columns.Select(c => $"`{c}`").ToArray())} FROM {TableName}";

//                using (var reader = command.ExecuteReader())
//                {
//                    while (reader.Read())
//                    {
//                        var model = new TModel();
//                        Fill(ref model, reader);
//                        _models.Add(model);
//                    }
//                }

//                Refreshed.FireEvent(new CachedDriverRefreshEventArgs<TModel>(this, DriverOperation.Loaded, _models.ToArray()), this);
//            }
//            //Logger.LogSuccess($"{Models.Count} {typeof(TModel).Name} models loaded!");
//        }

//        private void Fill(ref TModel model, DbDataReader reader)
//        {
//            foreach (var property in Properties)
//            {
//                object value = reader[property.Name];
//                if (typeof(IModel).IsAssignableFrom(property.PropertyType))
//                {
//                    var driver = _drivers.First(d => d.ModelType == property.PropertyType);
//                    value = driver.InternalGetModel(m => m.ID == Convert.ToInt32(value));
//                }
//                else if (property.PropertyType.IsArray || typeof(ICollection).IsAssignableFrom(property.PropertyType))
//                {
//                    var ds = GetModelIDs(value);

//                    if (property.PropertyType.IsArray)
//                    {
//                        var driver = _drivers.First(d => d.ModelType == property.PropertyType.GetElementType());
//                        value = driver.InternalGetModels(m => ds.Any(i => i == m.ID));
//                    }
//                    else if (typeof(IList).IsAssignableFrom(property.PropertyType))
//                    {
//                        var driver = _drivers.First(d => d.ModelType == property.PropertyType.GetGenericArguments()[0]);
//                        var items = driver.InternalGetModels(m => ds.Any(i => i == m.ID)) as Array;

//                        var temp = (IList)Activator.CreateInstance(property.PropertyType);

//                        for (int i = 0; i < items.Length; i++)
//                            temp.Add(items.GetValue(i));
//                        value = temp;
//                    }
//                }

//                property.SetValue(model, Convert.ChangeType(value, property.PropertyType));
//            }
//        }

//        protected void RegisterModelDriver<TDriver>() where TDriver : class, ICachedDriver, new()
//        {
//            if (!_drivers.Any(d => d is TDriver) && ComponentFactory.Enable<TDriver>())
//                _drivers.Add(ComponentFactory.GetComponent<TDriver>());
//        }

//        DelayMode IUpdater.DelayMode { get { return DelayMode.DelayBefore; } }
//        int IUpdater.Interval => 1000 * 30;
//        void IThread.Start() => _logger.LogSuccess($"Hibernated driver {typeof(TModel).Name} has been started!");

//        void IThread.End()
//        {
//            lock (_syncLock)
//            {

//            }
//        }

//        bool IThread.Run()
//        {
//            try
//            {
//                lock (_syncLock)
//                {
//                    LoadModels();
//                }
//                return true;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogFatal(ex);
//                return false;
//            }
//        }

//        IModel ICachedDriver.InternalGetModel(Predicate<IModel> condition)
//        { lock (_syncLock) { return _models.FirstOrDefault(T => condition == null ? true : condition(T)); } }

//        public static TModel GetModel(Predicate<TModel> condition = null)
//        => (TModel)Driver.InternalGetModel(m => condition == null ? true : condition(m as TModel)) ?? null;

//        IModel[] ICachedDriver.InternalGetModels(Predicate<IModel> condition)
//        { lock (_syncLock) { return _models.Where(T => condition == null ? true : condition(T)).ToArray(); } }

//        public static TModel[] GetModels(Predicate<TModel> condition = null)
//        => (TModel[])Driver.InternalGetModels(m => condition == null ? true : condition(m as TModel)) ?? new TModel[0];

//        bool ICachedDriver.InternalHasModel(Predicate<IModel> condition)
//        { lock (_syncLock) { return _models.Any(m => condition == null ? true : condition(m)); } }

//        public static bool HasModel(Predicate<TModel> condition = null)
//        => Driver.InternalHasModel(m => condition == null ? true : condition(m as TModel));

//        bool ICachedDriver.InternalHasModel(out IModel model, Predicate<IModel> condition)
//        {
//            lock (_syncLock)
//            {
//                model = _models.FirstOrDefault(m => condition == null ? true : condition(m));
//                return model != null;
//            }
//        }

//        public static bool HasModel(out TModel model, Predicate<TModel> condition = null)
//        {
//            IModel output = null;
//            var result = Driver.InternalHasModel(out output, m => condition == null ? true : condition(m as TModel));

//            model = output == null ? null : (TModel)output;
//            return result;
//        }

//        int ICachedDriver.InternalCount(Predicate<IModel> condition)
//        { lock (_syncLock) { return _models.Count(m => condition == null ? true : condition(m)); } }

//        public static int Count(Predicate<TModel> condition = null)
//        => Driver.InternalCount(m => condition == null ? true : condition(m as TModel));
//    }
//}