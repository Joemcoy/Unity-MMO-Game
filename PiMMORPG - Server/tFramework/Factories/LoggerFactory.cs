using System;
using System.Collections.Generic;

namespace tFramework.Factories
{
    using Bases;
    using Enums;
    using EventArgs;
    using Interfaces;

    public class LoggerFactory : ISingleton, IUpdater
    {
        class LogMessage
        {
            public ILogger Logger { get; private set; }
            public LogType Type { get; set; }
            public string Message { get; set; }

            public LogMessage(ILogger logger, LogType type, string message)
            {
                Logger = logger;
                Type = type;
                Message = message;
            }
        }

        static volatile object _syncLock = new object();
        Dictionary<string, ILogger> _loggers;
        Queue<LogMessage> _messages;

        public static Type BaseType { get; set; }
        public static bool EventHandled
        {
            get
            {
                return OnLog != null;
            }
        }

        int IUpdater.Interval { get { return 10; } }
        DelayMode IUpdater.DelayMode { get { return DelayMode.DelayAfter; } }

        public static event EventHandler<LogEventArgs> OnLog;

        void ISingleton.Created()
        {
            lock (_syncLock)
            {
                if (BaseType == null)
                    BaseType = typeof(BaseLogger);

                _loggers = new Dictionary<string, ILogger>();
                _messages = new Queue<LogMessage>();
                ThreadFactory.Start(this);
            }
        }

        void ISingleton.Destroyed()
        {
            lock (_syncLock)
            {
#if DEBUG
                //GetLogger(this).LogWarning("Destroying LoggerFactory with {0} loggers!", _loggers.Count);
#endif
                _loggers.Clear();

                ThreadFactory.Stop(this);
            }
        }

        public static ILogger GetLogger(object instance) { return instance == null ? GetLogger("NULL") : GetLogger(instance.GetType()); }
        public static ILogger GetLogger<T>() { return GetLogger(typeof(T)); }
        public static ILogger GetLogger(Type T) { return GetLogger(T.Name); }

        public static ILogger GetLogger(string name)
        {
            var factory = SingletonFactory.GetSingleton<LoggerFactory>();

            lock (_syncLock)
            {
                ILogger logger = null;
                if (!factory._loggers.TryGetValue(name, out logger))
                {
                    logger = (ILogger)Activator.CreateInstance(BaseType, name);
                    factory._loggers.Add(name, logger);
                }
                return logger;
            }
        }

        public static void EnqueueLog(ILogger logger, LogType type, string message)
        {
            var factory = SingletonFactory.GetSingleton<LoggerFactory>();

#if !UNITY_STANDALONE
            lock (_syncLock)
                factory._messages.Enqueue(new LogMessage(logger, type, message));
#else
            if(OnLog != null)
                OnLog.Invoke(factory, new LogEventArgs(logger, type, message));
#endif

        }

        void IThread.Start()
        {

        }

        bool IThread.Run()
        {
            while (OnLog != null && _messages.Count > 0)
            {
                lock (_syncLock)
                {
                    var message = _messages.Dequeue();
                    OnLog.Invoke(this, new LogEventArgs(message.Logger, message.Type, message.Message));
                }
            }
            return true;
        }

        void IThread.End()
        {
            if (OnLog != null)
            {
                lock (_syncLock)
                {
                    while (_messages.Count > 0)
                    {
                        var message = _messages.Dequeue();
                        OnLog.Invoke(this, new LogEventArgs(message.Logger, message.Type, message.Message));
                    }
                }
            }
            else
                _messages.Clear();
        }
    }
}