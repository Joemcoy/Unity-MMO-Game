using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace tFramework.Helper
{
    public static class IdHelper
    {
        public static string GetName(object target) { return GetName(target.GetType()); }
        public static string GetName<T>() { return GetName(typeof(T)); }
        public static string GetName(Type target) { return target.Name; }
    }
}
