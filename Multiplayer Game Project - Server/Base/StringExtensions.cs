using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base
{
    public static class StringExtensions
    {
        public static string Format(this string Target, params object[] Arguments)
        {
            return string.Format(Target, Arguments);
        }
    }
}
