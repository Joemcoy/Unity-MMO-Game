using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

namespace tFramework.Factories
{
    using Interfaces;

    public class CommandFactory : ISingleton
    {
        Dictionary<string, ICommand> commands;
        static ILogger logger;
        static readonly object syncLock = new object();

        void ISingleton.Created()
        {
            commands = new Dictionary<string, ICommand>();
            logger = LoggerFactory.GetLogger(this);
        }

        void ISingleton.Destroyed()
        {
            commands.Clear();
        }
        
        public static void RegisterCommands<TCommand>(params Assembly[] assemblies) where TCommand : class, ICommand
        {
            if (assemblies.Length == 0)
                assemblies = new[] { Assembly.GetCallingAssembly() };

            foreach (var type in assemblies.SelectMany(a => a.GetTypes().Where(t => !t.IsAbstract && !t.IsInterface && (typeof(TCommand).IsAssignableFrom(t) || typeof(TCommand).IsSubclassOf(t)))))
            {
                var command = Activator.CreateInstance(type) as TCommand;
                RegisterCommand(command);
            }
        }

        public static void RegisterCommand<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            lock (syncLock)
            {
                var instance = SingletonFactory.GetSingleton<CommandFactory>();
                instance.commands[command.Name] = command;
            }
            logger.LogSuccess("Command '{0}' has been loaded!", command.Name);
        }

        public static TCommand[] GetCommands<TCommand>() where TCommand : class, ICommand
        {
            lock (syncLock)
            {
                var instance = SingletonFactory.GetSingleton<CommandFactory>();
                return instance.commands.Values.OfType<TCommand>().ToArray();
            }
        }

        public static bool ExecuteCommand(string commandLine, object caller = null)
        {
            lock (syncLock)
            {
                var instance = SingletonFactory.GetSingleton<CommandFactory>();
                var ps = commandLine.Split(' ');
                var name = ps[0];

                ICommand command;
                if (instance.commands.TryGetValue(name, out command))
                {
                    var args = ps.Skip(1).ToArray();
                    return command.Parse(caller, args) && command.Execute();
                }
                return false;
            }
        }
    }
}