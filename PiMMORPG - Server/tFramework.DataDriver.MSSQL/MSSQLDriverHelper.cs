using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tFramework.DataDriver.MSSQL
{
    using Data.Interfaces;

    public static class MSSQLDriverHelper
    {
        public static string GetTypeName(Type target)
        {
            if (target.IsEnum)
                return GetTypeName(Enum.GetUnderlyingType(target));
            else if (target == typeof(string) || typeof(IEnumerable).IsAssignableFrom(target))
                return "VARCHAR(MAX)";
            else if (target == typeof(byte))
                return "TINYINT";
            else if (target == typeof(short) || target == typeof(ushort))
                return "SMALLINT";
            else if (target == typeof(int)|| target == typeof(uint) || typeof(IModel).IsAssignableFrom(target))
                return "INT";
            else if (target == typeof(long) || target == typeof(ulong))
                return "BIGINT";
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
            else if (!target.IsValueType || Nullable.GetUnderlyingType(target) != null)
            {
                var g = Nullable.GetUnderlyingType(target);
                return GetTypeName(g);
            }

            throw new NotSupportedException();
        }

        public static int[] GetModelIDs(object value)
            => Convert.ToString(value).Split(',').Select(s => Convert.ToInt32(s)).ToArray();

        public static object GetIdsValue(int[] ds)
            => string.Join(",", ds.Select(i => Convert.ToString(i)).ToArray());
    }
}
