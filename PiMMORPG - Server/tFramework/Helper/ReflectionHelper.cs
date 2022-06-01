using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;

namespace tFramework.Helper
{
    public static class ReflectionHelper
    {
        /// <summary>
        /// The standard flags in method calls.
        /// </summary>
        public const BindingFlags DefaultFlags = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        /// <summary>
        /// Invokes the method if it is found, and if it is not static, only if the instance of the base type is informed.
        /// </summary>
        /// <param name="baseClass">The base of method that search</param>
        /// <param name="methodName">Name of the method</param>
        /// <param name="parameters">Parameters of the method</param>
        /// <returns>Returns the return value if the method is a function.</returns>
        public static object CallMethod(Type baseClass, string methodName, params object[] parameters)
        {
            return CallMethod(baseClass, methodName, null, parameters);
        }

        /// <summary>
        /// Invokes the method if it is found, and if it is not static, only if the instance of the base type is informed.
        /// </summary>
        /// <param name="baseClass">The base of method that search</param>
        /// <param name="methodName">Name of the method</param>
        /// <param name="caller">Instance of base class</param>
        /// <param name="parameters">Parameters of the method</param>
        /// <returns>Returns the return value if the method is a function.</returns>
        public static object CallMethod(Type baseClass, string methodName, object caller, params object[] parameters)
        {
            return CallMethod(baseClass, methodName, caller, DefaultFlags, parameters);
        }

        /// <summary>
        /// Invokes the method if it is found, and if it is not static, only if the instance of the base type is informed.
        /// </summary>
        /// <param name="baseClass">The base of method that search</param>
        /// <param name="methodName">Name of the method</param>
        /// <param name="caller">Instance of base class</param>
        /// <param name="flags">The method flags, default is Static, Intance, Public and NonPublic</param>
        /// <param name="parameters">Parameters of the method</param>
        /// <returns>Returns the return value if the method is a function.</returns>
        public static object CallMethod(Type baseClass, string methodName, object caller = null, BindingFlags flags = DefaultFlags, params object[] parameters)
        {
            object returnValue = null;
            var method = baseClass.GetMethod(methodName, flags);
            if (method != null)
            {
                if(!method.IsStatic && caller != null)
                    returnValue = method.Invoke(caller, parameters);
            }
            return returnValue;
        }

        public static PropertyInfo ExtractProperty<TSource, TProperty>(this Expression<Func<TSource, TProperty>> expr, bool CheckSubClass = false)
        {
            var type = typeof(TSource);
            var member = expr.Body as MemberExpression;
            if (member == null)
            {
                var unary = expr.Body as UnaryExpression;
                if (unary != null)
                    member = unary.Operand as MemberExpression;
                else
                    throw new InvalidOperationException();
            }

            var property = member.Member as PropertyInfo;
            if (property == null)
                throw new InvalidOperationException();

            var reflected = property.ReflectedType;
            if (CheckSubClass && type != reflected && !type.IsSubclassOf(reflected))
                throw new InvalidOperationException();

            return property;
        }
    }
}