//#define MONO

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Base.Data.Interfaces;
using Base.Data.Abstracts;
using System.Reflection;

namespace Base.Factories
{
    public class CommandFactory<TCommand> : ISingleton, IComponent
        where TCommand : ACommand
    {
        Dictionary<string, TCommand> Commands;

        public void Create()
        {
            Commands = new Dictionary<string, TCommand>();
        }

        public void Destroy()
        {
            Commands.Clear();
        }

        public bool Enable()
        {
            try
            {
                foreach(Type T in AppDomain.CurrentDomain.GetAssemblies().SelectMany(A => A.GetTypes()))
                {
                    if(typeof(TCommand).IsAssignableFrom(T) && !T.IsAbstract)
                    {
                        TCommand Command = (TCommand)Activator.CreateInstance(T);
                        Commands[Command.Name] = Command;
                    }
                }

                LoggerFactory.GetLogger(this).LogInfo("Loaded {0} of {1} command!", Commands.Count, typeof(TCommand).Name);
                return true;
            }
            catch(Exception ex)
            {
                LoggerFactory.GetLogger(this).LogFatal(ex);
                return false;
            }
        }

        public bool Disable()
        {
            try
            {
                return true;
            }
            catch (Exception ex)
            {
                LoggerFactory.GetLogger(this).LogFatal(ex);
                return false;
            }
        }

        public KeyValuePair<string, object> CreateParameter(string Name, object Value)
        {
            return new KeyValuePair<string, object>(Name, Value);
        }

        public bool ExecuteCommand(string CommandLine, params KeyValuePair<string, object>[] Parameters)
        {
            string[] CommandData = CommandLine.Split(' ');
            string Name = CommandData.First();
            string[] Args = CommandData.Skip(1).ToArray();

            if (Commands.ContainsKey(Name))
            {
                TCommand Command = Commands[Name];

                foreach (KeyValuePair<string, object> Parameter in Parameters)
                    Command.SetParameter(Parameter.Key, Parameter.Value);

                bool Result = Command.Execute(Args);
                Command.ClearParameters();

                return Result;
            }
            else
                return false;
        }

        public TCommand[] GetCommands()
        {
            return Commands.Values.ToArray();
        }
    }
}
