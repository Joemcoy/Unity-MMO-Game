using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.IO;

using Base.Data.Enums;
using Base.Data.Abstracts;
using Base.Data.Interfaces;
using Base.Data.EventArgs;
using Base.Data.DispatcherBases;

using Base.Configurations;
using Base.Factories.Loggers;

namespace Base.Factories
{
    public class LoggerFactory : ASingleton<LoggerFactory>, IUpdater
    {
        static volatile object syncLock = new object();
        Dictionary<string, ILogger> Loggers;
        Queue<Action> LogQueue;

        public int Interval { get; private set; }
        public static Type LoggerType { get; set; }

        public static event EventHandler<LoggerEventArgs> OnLog;

        protected override void Created()
        {
			#if !UNITY_5
            try
            {
                int Width = (Console.LargestWindowWidth * 60) / 100;
                int Height = (Console.LargestWindowHeight * 50) / 100;

                Console.SetWindowSize(Width, Height);
            }
            catch (Exception) { }
			#endif

            if(LoggerType == null)
                LoggerType = typeof(EventLogger);

            LogQueue = new Queue<Action>();
            Loggers = new Dictionary<string, ILogger>();

            UpdaterFactory.Start(this);
        }

        protected override void Destroyed()
        {
            UpdaterFactory.Stop(this);
        }

        public void Start()
        {
            //GetLogger(this).LogInfo("LoggerFactory has been started!");
        }

        public void Loop()
        {
            //lock (syncLock)
            //{
            Interval = IntervalConfiguration.LoggerInterval;

            if (LogQueue.Count > 0)
            {
                var Action = LogQueue.Dequeue();
                if (Action != null) Action.Invoke();
            }
            //}
        }

        public void End()
        {
            GetLogger(this).LogWarning("LoggerFactory has been stopped!");
            
            while(LogQueue.Count > 0)
            {
                var LogAction = LogQueue.Dequeue();

                if (LogAction != null)
                    LogAction.Invoke();

            }
        }

        public static ILogger GetLogger<Type>()
        {
            return GetLogger(typeof(Type));
        }

        public static ILogger GetLogger(Type Type)
        {
            return GetLogger(Type == null ? "NULL TYPE" : Type.Name);
        }

        public static ILogger GetLogger(object Value)
        {
            return Value == null ? GetLogger("NULL") : GetLogger(Value.GetType());
        }

        public static ILogger GetLogger(string Name)
        {
            //.NET 4.5
            //Name = Name.Substring(Name.LastIndexOf(Path.DirectorySeparatorChar) + 1);
            //Name = Path.GetFileNameWithoutExtension(Name);

            lock (syncLock)
            {
                if (!Instance.Loggers.ContainsKey(Name))
                {
                    var Logger = (ILogger)Activator.CreateInstance(LoggerType, Name);
                    Instance.Loggers.Add(Name, Logger);
                }
                return Instance.Loggers[Name];
            }
        }

        public static void FireOnLog(ILogger Logger, LogType Type, string Message)
        {
            lock (syncLock)
            {
                var Factory = SingletonFactory.GetInstance<LoggerFactory>();
                //Factory.DispatchBase(d => d.OnLog(Logger, Type, Message));

                if (OnLog != null)
                    OnLog(Factory, new LoggerEventArgs(Logger, Type, Message));
            }
        }

        public static void EnqueueAction(Action WriteAction)
        {
            lock (syncLock)
            {
                var Factory = SingletonFactory.GetInstance<LoggerFactory>();
                Factory.LogQueue.Enqueue(WriteAction);
            }
        }
    }
}