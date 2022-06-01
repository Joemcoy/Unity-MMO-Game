using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Base
{
    public static class ReflectionExtensions
    {
        public static bool HasAttribute<TAttribute>(this MemberInfo Member, bool Inherit = false) where TAttribute : Attribute
        {
            TAttribute Attribute;
            return HasAttribute<TAttribute>(Member, out Attribute, Inherit);
        }
        public static bool HasAttribute<TAttribute>(this MemberInfo Member, out TAttribute Attribute, bool Inherit = false) where TAttribute : Attribute
        {
            var Attributes = Member.GetCustomAttributes(typeof(TAttribute), Inherit);
            var Has = Attributes != null && Attributes.Length > 0 && Attributes.Any(T => T is TAttribute);
            Attribute = Has ? Attributes.Cast<TAttribute>().FirstOrDefault() : null;

            return Has;
        }
    }
}
