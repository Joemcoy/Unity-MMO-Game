using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using Base.Data.Interfaces;
using Base.Data.Attributes;
using Base.Factories;

namespace Base.Manager
{
    public class ArgumentManager
    {
        public const string Prefix = "--";
        static readonly ILogger Logger = LoggerFactory.GetLogger<ArgumentManager>();

        public static void Parse()
        {
            foreach (var Receiver in AppDomain.CurrentDomain.GetAssemblies().SelectMany(T => T.GetTypes()).Where(T => !T.IsInterface && !T.IsAbstract && typeof(IArgumentReceiver).IsAssignableFrom(T)))
            {
                Logger.LogInfo("Parsing {0}...", Receiver.Name);
                foreach (var Property in Receiver.GetProperties(BindingFlags.Static | BindingFlags.Public))
                {
                    ArgumentAttribute Argument;
                    if (Property.HasAttribute(out Argument))
                    {
                        object Value = null;
                        if (ExtractArgument(Argument.Name, ref Value))
                        {
                            if (Property.PropertyType == typeof(bool))
                                Value = true;
                            else
                                Value = Convert.ChangeType(Value, Property.PropertyType);

                            Logger.LogSuccess("Argument {0} has been parsed!", Argument.Name);
                            Property.SetValue(null, Value, null);
                        }
                    }
                }
            }
        }

        static bool ExtractArgument(string Name, ref object Value)
        {
            var Argument = Environment.GetCommandLineArgs().FirstOrDefault(A => A.IndexOf(Prefix + Name) > -1);
            if (Argument == null)
                return false;
            else if (Argument.IndexOf("=") > -1)
                Value = Argument.Substring(Argument.IndexOf("=") + 1);
            return true;
        }
    }
}