using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using MySql.Data;
using MySql.Data.MySqlClient;

using Base.Factories;
using Base.Data.Interfaces;
using Game.Data.Attributes;

using System.Configuration;
using System.Reflection;
using Server.Configuration;
using Base.Configurations;

namespace Data.Server.BaseControllers
{
    public class BaseMysqlController : IBaseController
    {
        List<PropertyInfo> Properties;
        List<Action<IBaseController>> AfterLoadCallbacks, AfterSaveCallbacks;
        List<Action<IBaseController, IModel>> AfterSaveModelCallbacks;

        Dictionary<int, Tuple<bool, IModel>> CurrentModels;
        static object syncLock = new object();
        static bool DatabaseCheck = false;

        public event EventHandler TableCreated;

        public string TableName { get; set; }
        public Type ModelType { get; set; }


        public string[] Columns
        {
            get
            {
                if (Properties == null) LoadProperties();
                return Properties.Select(P =>
                {
                    ColumnDataAttribute Data = GetColumnData(P);
                    if (Data == null || string.IsNullOrEmpty(Data.Name))
                        return P.Name;
                    else
                        return Data.Name;
                }).ToArray();
            }
        }

        public string[] UniqueColumns
        {
            get
            {
                if (Properties == null) LoadProperties();
                return Properties.Where(P => P.GetCustomAttributes(typeof(ColumnDataAttribute), false).Length != 0 && P.GetCustomAttributes(typeof(ColumnDataAttribute), false).Cast<ColumnDataAttribute>().First().Unique)
                    .Select(P => P.Name).ToArray();
            }
        }

        public int Interval { get { return 60000; } }

        public BaseMysqlController()
        {
            AfterLoadCallbacks = new List<Action<IBaseController>>();
            AfterSaveCallbacks = new List<Action<IBaseController>>();
            AfterSaveModelCallbacks = new List<Action<IBaseController, IModel>>();
        }

        public void RegisterAfterLoadModelCallback(Action<IBaseController> Callback)
        {
            AfterLoadCallbacks.Add(Callback);
        }

        public void RegisterAfterSaveCallback(Action<IBaseController> Callback)
        {
            AfterSaveCallbacks.Add(Callback);
        }

        public void RegisterAfterSaveModelCallback(Action<IBaseController, IModel> Callback)
        {
            AfterSaveModelCallbacks.Add(Callback);
        }

        void LoadProperties()
        {
            Properties = new List<PropertyInfo>();
            foreach(PropertyInfo Property in ModelType.GetProperties().Where(P => P.CanWrite && P.CanRead))
            {
                var Attr = Property.GetCustomAttributes(typeof(NonColumnAttribute), false);

                if ((Attr == null || Attr.Length == 0) && CheckProperty(Property.PropertyType))
                    Properties.Add(Property);
            }
        }

        public MySqlConnection CreateConnection()
        {
            MySqlConnection C = new MySqlConnection(DatabaseConfiguration.ConnectionString);
            C.Open();

            if (!DatabaseCheck)
            {
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = C;
                    Command.CommandText = string.Format("CREATE SCHEMA IF NOT EXISTS {0}", DatabaseConfiguration.Database);
                    Command.ExecuteNonQuery();
                    DatabaseCheck = true;
                }
            }
            C.ChangeDatabase(DatabaseConfiguration.Database);

            return C;
        }

        public bool Open()
        {
            try
            {
                if (!ComponentFactory.Enable<DatabaseConfiguration>())
                    return false;
                else if (!ComponentFactory.Enable<IntervalConfiguration>())
                    return false;

                CurrentModels = new Dictionary<int, Tuple<bool, IModel>>();
                CreateTable();
                return true;
            }
            catch (Exception ex)
            {
                LoggerFactory.GetLogger(this).LogFatal(ex);
                return false;
            }
        }

        public bool Close()
        {
            try
            {
                SaveData();
                CurrentModels.Clear();
                return true;
            }
            catch (Exception ex)
            {
                LoggerFactory.GetLogger(this).LogFatal(ex);
                return false;
            }
        }

        void CreateTable()
        {
            if (!CheckTable())
            {
                LoggerFactory.GetLogger(this).LogInfo($"Auto-creating table {TableName}...");

                using (MySqlConnection Connection = CreateConnection())
                {
                    using (MySqlCommand Command = new MySqlCommand())
                    {
                        Command.Connection = Connection;

                        string TableData = string.Empty;
                        foreach (string ColumnName in Columns)
                        {
                            TableData += $"{ColumnName} {GetColumnType(ColumnName)} NOT NULL";
                            if (ColumnName == "ID")
                                TableData += " PRIMARY KEY AUTO_INCREMENT";

                            object DefaultValue = GetDefaultValue(ColumnName);
                            if (DefaultValue != null)
                            {
                                TableData += $" DEFAULT @{ColumnName}";
                                Command.Parameters.AddWithValue($"@{ColumnName}", DefaultValue);
                            }
                            TableData += ",";
                        }

                        if (UniqueColumns.Length > 0)
                            TableData += string.Join(",", UniqueColumns.Select(U => $"UNIQUE({U})").ToArray());

                        if (TableData.EndsWith(","))
                            TableData = TableData.Substring(0, TableData.Length - 1);

                        Command.CommandText = $"CREATE TABLE {TableName} ({TableData});";
                        Command.ExecuteNonQuery();

                        Base.Helpers.EventHelper.FireEvent(TableCreated, this);
                    }
                }
            }
        }

        bool CheckTable()
        {
            int Result = 0;
            using (MySqlConnection Connection = CreateConnection())
            {
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = Connection;
                    Command.CommandText = "SELECT COUNT(TABLE_NAME) FROM `INFORMATION_SCHEMA`.`TABLES` WHERE TABLE_SCHEMA = (SELECT DATABASE()) AND TABLE_NAME = @Name";
                    Command.Parameters.AddWithValue("@Name", TableName);

                    Result = Convert.ToInt32(Command.ExecuteScalar());
                }
            }
            return Result > 0;
        }

        public bool Disable()
        {
            try
            {
                return Close();
            }
            catch (Exception ex)
            {
                LoggerFactory.GetLogger(this).LogFatal(ex);
                return false;
            }
        }      

        protected ColumnDataAttribute GetColumnData(PropertyInfo Property)
        {
            object[] Attributes = Property.GetCustomAttributes(typeof(ColumnDataAttribute), false);
            return Attributes.Length == 0 ? null : ((ColumnDataAttribute)Attributes[0]);
        }

        PropertyInfo GetProperty(string Name)
        {
            return Properties.Where(P =>
            {
                var Attr = P.GetCustomAttributes(typeof(NonColumnAttribute), false);
                if ((Attr == null || Attr.Length == 0) && CheckProperty(P.PropertyType))
                {
                    ColumnDataAttribute Data = GetColumnData(P);
                    return Data == null || string.IsNullOrEmpty(Data.Name) ? P.Name == Name : Data.Name == Name;
                }
                else
                    return false;
            }).FirstOrDefault();
        }

        public string GetColumnType(string Name)
        {
            PropertyInfo Property = GetProperty(Name);
            return Property == null ? null : GetType(Property);
        }

        bool CheckProperty(Type PropertyType)
        {
            return
                PropertyType.IsPrimitive || PropertyType.IsEnum || PropertyType == typeof(string) ||
                PropertyType == typeof(Guid) || PropertyType == typeof(DateTime);
        }

        string GetType(PropertyInfo Property)
        {
            ColumnDataAttribute Data = GetColumnData(Property);
            if (Property.PropertyType == typeof(short))
                return "TINYINT";
            else if (Property.PropertyType == typeof(int))
                return "INT";
            else if (Property.PropertyType == typeof(long))
                return "BIGINT";
            else if (Property.PropertyType == typeof(ushort))
                return "TINYINT UNSIGNED";
            else if (Property.PropertyType == typeof(uint))
                return "INT UNSIGNED";
            else if (Property.PropertyType == typeof(ulong))
                return "BIGINT UNSIGNED";
            else if (Property.PropertyType == typeof(float))
                return "FLOAT";
            else if (Property.PropertyType == typeof(double))
                return "DOUBLE";
            else if (Property.PropertyType == typeof(bool))
                return "BOOL";
            else if (Property.PropertyType == typeof(string))
                return string.Format("VARCHAR({0})", Data == null || Data.MaximumLength == 0 ? 255 : Data.MaximumLength);
            else if (Property.PropertyType == typeof(DateTime))
                return "TIMESTAMP";
            else if (Property.PropertyType.IsEnum)
                return "VARCHAR(80)";
            else if (Property.PropertyType == typeof(Guid))
                return "BINARY(16)";
            else
                return null;
        }

        public object GetDefaultValue(string Name)
        {
            PropertyInfo Property = GetProperty(Name);            

            ColumnDataAttribute Data = GetColumnData(Property);
            return Data == null ? null : Data.DefaultValue;
        }

        object GetValue(IModel Model, string Column)
        {
            PropertyInfo Property = GetProperty(Column);

            return Property.GetValue(Model, null);
        }

        void SetValue(IModel Model, string Column, object Value)
        {
            PropertyInfo Property = GetProperty(Column);

            if (Property.PropertyType == typeof(DateTime))
                Value = Convert.ToDateTime(Value);
            else if (Property.PropertyType.IsEnum)
                Value = Enum.Parse(Property.PropertyType, Value.ToString());
            else if (Property.PropertyType == typeof(Guid))
                Value = new Guid((byte[])Value);
            else
                Value = Convert.ChangeType(Value, Property.PropertyType);

            Property.SetValue(Model, Value, null);
        }

        public void AddModel(IModel Model)
        {
            SaveModel(Model);
            CurrentModels[Model.ID] = new Tuple<bool, IModel>(true, Model);
        }

        public void RemoveModel(IModel Model)
        {
            lock (syncLock)
            {                
                if (CurrentModels.ContainsKey(Model.ID))
                {
                    using (MySqlConnection Connection = CreateConnection())
                    {
                        using (MySqlCommand Command = new MySqlCommand(string.Format("DELETE FROM {0} WHERE ID = @ID", TableName)))
                        {
                            Command.Parameters.AddWithValue("@ID", Model.ID);

                            Command.Connection = Connection;
                            Command.ExecuteNonQuery();
                        }
                    }

                    CurrentModels.Remove(Model.ID);
                }
            }
        }

        public void UpdateModel(IModel Model)
        {
            lock (syncLock)
            {
                SaveModel(Model);
                CurrentModels[Model.ID] = new Tuple<bool, IModel>(false, Model);
            }
        }

        public bool LoadData()
        {
            try
            {
                lock (syncLock)
                {
                    using (MySqlConnection Connection = CreateConnection())
                    {
                        using (MySqlCommand Command = new MySqlCommand(string.Format("SELECT {0} FROM {1}", string.Join(",", Columns), TableName)))
                        {
                            Command.Connection = Connection;

                            //CurrentModels.Clear();
                            using (MySqlDataReader Reader = Command.ExecuteReader())
                            {
                                while (Reader.Read())
                                {
                                    IModel Model = LoadModel(Reader);

                                    CurrentModels[Model.ID] = new Tuple<bool, IModel>(true, Model);
                                }
                            }

                            AfterLoadCallbacks.ForEach(c => c(this));
                        }
                    }
                }

                LoggerFactory.GetLogger(this).LogInfo($"Loaded {CurrentModels.Count} {ModelType.Name.Replace("Model", string.Empty)} models!");
                return true;
            }
            catch (Exception ex)
            {
                LoggerFactory.GetLogger(this).LogFatal(ex);
                return false;
            }
        }

        private IModel LoadModel(MySqlDataReader Reader)
        {
            IModel Model = (IModel)Activator.CreateInstance(ModelType);

            foreach (string Name in Columns)
            {
                object Value = Reader[Name];

                SetValue(Model, Name, Value);
            }

            return Model;
        }

        public void SaveData()
        {
            try
            {
                lock (syncLock)
                {
                    foreach (IModel Model in CurrentModels.Values.Where(I => !I.Item1).Select(I => I.Item2))
                    {
                        SaveModel(Model);
                    }

                    foreach (Action<IBaseController> Callback in AfterSaveCallbacks)
                        Callback(this);
                }
                //LoggerFactory.GetLogger(this).LogInfo($"Saved {CurrentModels.Count} {Controller.ModelType.Name} models!");
            }
            catch (Exception ex)
            {
                LoggerFactory.GetLogger(this).LogFatal(ex);
            }
        }

        public virtual IModel SaveModel(IModel Model)
        {
            string ValuesDefinition = string.Empty;
            using (MySqlConnection Connection = CreateConnection())
            {
                using (MySqlCommand Command = new MySqlCommand())
                {

                    string[] Columns = this.Columns;

                    foreach (string Name in Columns)
                    {
                        object Value = GetValue(Model, Name) ?? GetDefaultValue(Name);
                        if (Value != null)
                        {
                            if (Value.GetType().IsEnum)
                                Value = Enum.GetName(Value.GetType(), Value);
                            else if (Value.GetType() == typeof(DateTime))
                                Value = ((DateTime)Value).ToString("yyyy-MM-dd HH:mm:ss");
                            else if (Value.GetType() == typeof(bool))
                                Value = ((bool)Value) ? 1 : 0;
                            else if (Value.GetType() == typeof(Guid))
                                Value = ((Guid)Value).ToByteArray();
                        }
                        else
                        {
                            var Property = GetProperty(Name);
                            if (Property.PropertyType == typeof(string)) Value = string.Empty;
                            else if (Property.PropertyType.IsValueType) Value = Activator.CreateInstance(Property.PropertyType);
                        }

                        if (Name != "ID" && Value != null)
                        {
                            if (ValuesDefinition != string.Empty)
                                ValuesDefinition += ", ";
                            ValuesDefinition += string.Format("@{0}", Name);
                            Command.Parameters.AddWithValue("@" + Name, Value);
                        }
                    }

                    Command.Connection = Connection;
                    if (!CheckIdentity(Model))
                    {
                        Command.CommandText = string.Format("INSERT INTO {0} ({1}) VALUES({2});SELECT LAST_INSERT_ID()", TableName, ValuesDefinition.Replace("@", ""), ValuesDefinition);
                        Model.ID = Convert.ToInt32(Command.ExecuteScalar());
                    }
                    else
                    {
                        Command.CommandText = string.Format("UPDATE {0} SET {1} WHERE ID = {2}", TableName, string.Join(",", Columns.Where(C => C != "ID").Select(C => string.Format("{0}=@{0}", C)).ToArray()), Model.ID);
                        Command.ExecuteNonQuery();
                    }

                    foreach (Action<IBaseController, IModel> Callback in AfterSaveModelCallbacks)
                        Callback(this, Model);

                    return Model;
                }
            }
        }

        bool CheckIdentity(IModel Model)
        {
            int Count = 0;
            using (MySqlConnection Connection = CreateConnection())
            {
                using (MySqlCommand Command = new MySqlCommand())
                {
                    Command.Connection = Connection;
                    Command.CommandText = string.Format("SELECT COUNT(ID) FROM {0} WHERE ID = {1}", TableName, Model.ID);

                    Count = Convert.ToInt32(Command.ExecuteScalar());
                    //LoggerFactory.GetLogger(this).LogInfo($"{Controller.TableName} identity for {Model.ID} count => {Count}");
                }
            }
            return Count > 0;
        }

        public TModel GetModel<TModel>(int ID) where TModel : IModel
        {
            IModel Model = GetModel(ID);
            return Model == null ? default(TModel) : (TModel)Model;
        }

        public TModel GetModel<TModel>(Predicate<TModel> Condition) where TModel : IModel
        {
            return (TModel)CurrentModels.Values.Where(M => Condition((TModel)M.Item2)).Select(M => M.Item2).FirstOrDefault();
        }

        public TModel[] GetModels<TModel>() where TModel : IModel
        {
            IModel[] Models = GetModels();
            return Models == null ? null : Models.Cast<TModel>().ToArray();
        }

        public IModel[] GetModels()
        {
            return CurrentModels.Values.Select(M => M.Item2).ToArray();
        }

        public IModel GetModel(int ID)
        {
            LoggerFactory.GetLogger(this).LogInfo("{0}", ID);
            return CurrentModels[ID].Item2;
        }

        public TModel[] GetModels<TModel>(Predicate<TModel> Condition) where TModel : IModel
        {
            return CurrentModels.Values.Where(M => Condition((TModel)M.Item2)).Select(M => M.Item2).Cast<TModel>().ToArray();
        }
    }
}