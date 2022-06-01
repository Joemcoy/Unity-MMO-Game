using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace tFramework.Extensions
{
    using Factories;
    using Interfaces;

    public static class FrameworkExtensions
    {
        public static ILogger GetLogger(this string name) { return LoggerFactory.GetLogger(name); }
        public static ILogger GetLogger(this object instance) { return LoggerFactory.GetLogger(instance); }
        public static ILogger GetLogger(this Type type) { return LoggerFactory.GetLogger(type); }
        public static void Start(this IThread thread) { ThreadFactory.Start(thread); }
        public static void Stop(this IThread thread) { ThreadFactory.Start(thread); }
    }
}