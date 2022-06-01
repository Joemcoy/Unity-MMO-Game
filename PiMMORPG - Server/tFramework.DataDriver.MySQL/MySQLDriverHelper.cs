using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace tFramework.DataDriver.MySQL
{
    using Data.Interfaces;

    public static class MySqlDriverHelper
    {
        public static string GetTypeName(Type target)
        {
            if (target.IsEnum)
                return GetTypeName(Enum.GetUnderlyingType(target));
            else if (target == typeof(string) || typeof(IEnumerable).IsAssignableFrom(target))
                return "TEXT";
            else if (target == typeof(byte))
                return "TINYINT";
            else if (target == typeof(short))
                return "SMALLINT";
            else if (target == typeof(int))
                return "INT";
            else if (target == typeof(long))
                return "BIGINT";
            else if (target == typeof(ushort))
                return "SMALLINT UNSIGNED";
            else if (target == typeof(uint) || typeof(IModel).IsAssignableFrom(target))
                return "INT UNSIGNED";
            else if (target == typeof(ulong))
                return "BIGINT UNSIGNED";
            else if (target == typeof(float))
                return "FLOAT";
            else if (target == typeof(double))
                return "DOUBLE";
            else if (target == typeof(decimal))
                return "DECIMAL";
            else if (target == typeof(char))
                return "CHAR";
            else if (target == typeof(DateTime))
                return "DATETIME";
            else if (target == typeof(bool))
                return "BIT";
            else if (target == typeof(Guid))
                return "CHAR(36)";
            else if(!target.IsValueType || Nullable.GetUnderlyingType(target) != null)
            {
                var g = Nullable.GetUnderlyingType(target);
                return GetTypeName(g);
            }

            throw new NotSupportedException();
        }

        public static int[] GetModelIDs(object value)
            => Convert.ToString(value).Split(';').Select(s => Convert.ToInt32(s)).ToArray();

        public static object GetIdsValue(int[] ds)
            => string.Join(";", ds.Select(i => Convert.ToString(i)).ToArray());
    }
}