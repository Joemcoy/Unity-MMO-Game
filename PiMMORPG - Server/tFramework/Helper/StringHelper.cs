using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace tFramework.Helper
{
    public static class StringHelper
    {
        public static string SafeGetString(string Base)
        {
            return Base ?? "NULL";
        }

        public static string SafeGetString(object Base)
        {
            return Base == null ? "NULL" : Base.ToString();
        }

        public static string SafeGetString(Func<string> wrapper)
        {
            try
            {
                var str = wrapper();
                return SafeGetString(str);
            }
            catch (NullReferenceException) { return "NULL"; }
            catch (ObjectDisposedException) { return "DISPOSED"; }
            catch (Exception) { return "ERROR"; }
        }

        public static string ToTitleCase(this string Value)
        {
            return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Value);
        }
    }
}