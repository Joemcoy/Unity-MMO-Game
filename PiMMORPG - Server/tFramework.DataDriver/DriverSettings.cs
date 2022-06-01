using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace tFramework.DataDriver
{
    using Interfaces;
    using Helper;
    using Data.Interfaces;

    public enum OperationAlignment
    {
        Left,
        Right
    }

    public class DriverSettings<TModel> : IDisposable
        where TModel : IModel, new()
    {
        public class OperatorSettings
        {
            public OperationAlignment Alignment { get; set; }
            public string Keyword { get; set; }

            public OperatorSettings(string Keyword)
            {
                this.Keyword = Keyword;
                Alignment = OperationAlignment.Right;
            }
        }

        public class DriverMap
        {
            public bool UpdateOrRemove { get; }
            public IDriver Driver { get; }

            public DriverMap(IDriver driver, bool updateOrRemove)
            {
                Driver = driver;
                UpdateOrRemove = updateOrRemove;
            }
        }

        public bool AutoCreateDatabase { get; set; }
        public bool AutoCreateTable { get; set; }
        public char OpenDefinitionChar { get; set; }
        public char CloseDefinitionChar { get; set; }
        public bool MaintainConnection { get; set; }

        public OperatorSettings LimitSettings { get; }
        public OperatorSettings OffsetSettings { get; }
        public OperatorSettings PaginationSettings { get; }

        public string CheckDatabaseQuery { get; set; }
        public string CheckTableQuery { get; set; }
        public string CreateTableQuery { get; set; }
        public string LastInsertIDQuery { get; set; }
        
        private Dictionary<string, DriverMap> driverMaps;
        private Dictionary<string, string> mappedColumns;
        private Dictionary<string, char> separatorChars;
        private List<PropertyInfo> ignoredProperties;
        private IDriver<TModel> driver;

        public DriverSettings(IDriver<TModel> driver)
        {
            this.driver = driver;
            driverMaps = new Dictionary<string, DriverMap>();
            mappedColumns = new Dictionary<string, string>();
            separatorChars = new Dictionary<string, char>();
            ignoredProperties = new List<PropertyInfo>();

            LimitSettings = new OperatorSettings("LIMIT {0}");
            OffsetSettings = new OperatorSettings("OFFSET {0}");
            PaginationSettings = new OperatorSettings("OFFSET {0} LIMIT {1}");

            AutoCreateDatabase = true;
            AutoCreateTable = true;
            MaintainConnection = false;
            OpenDefinitionChar = '`';
            CloseDefinitionChar = '`';
        }

        public string GetColumn(string mapped, bool property = true)
        {
            return !property ?
                (mappedColumns.ContainsValue(mapped) ? mappedColumns.First(kp => kp.Value == mapped).Key : mapped) :
                (mappedColumns.ContainsKey(mapped) ? mappedColumns[mapped] : mapped);
        }

        public bool IsMapped(string name)
        {
            return driverMaps.ContainsKey(name);
        }

        public DriverMap GetMappedDriver(string name)
        {
            DriverMap map;
            return driverMaps.TryGetValue(name, out map) ? map : null;
        }

        public void SetSeparatorChar<TValue>(Expression<Func<TModel, TValue[]>> expr, char separator)
        {
            var property = expr.ExtractProperty();
            separatorChars[property.Name] = separator;
        }

        public char GetSeparatorChar(string propertyName)
        {
            char sep;
            return separatorChars.TryGetValue(propertyName, out sep) ? sep : ',';
        }

        public void Ignore(Expression<Func<TModel, object>> expr)
        {
            var property = expr.ExtractProperty();
            ignoredProperties.Add(property);
        }

        public bool IsIgnored(PropertyInfo property)
        {
            return ignoredProperties.Contains(property);
        }

        public void MapColumn(Expression<Func<TModel, object>> expr, string column)
        {
            var property = expr.ExtractProperty();
            mappedColumns[property.Name] = column;
        }

        public void MapDriver<TDriver>(Expression<Func<TModel, object>> expr, TDriver ctx, bool updateOrRemove = true)
            where TDriver : IDriver
        {
            var property = expr.ExtractProperty();
            var type = property.PropertyType;

            if (type.IsArray && type.GetElementType() != ctx.ModelType)
                throw new InvalidOperationException();
            else if (typeof(IList<>).IsAssignableFrom(type) && type.GetGenericArguments()[0] != ctx.ModelType)
                throw new InvalidOperationException();
            else if(type.IsArray && type.GetElementType() != ctx.ModelType)
                throw new InvalidOperationException();
            else if (!type.IsArray && ctx.ModelType != type)
                throw new InvalidOperationException();
            else
                driverMaps[property.Name] = new DriverMap(ctx, updateOrRemove);
        }

        public void Dispose()
        {
            foreach (var map in driverMaps)
                map.Value.Driver.Dispose();
            driverMaps.Clear();
        }
    }
}
