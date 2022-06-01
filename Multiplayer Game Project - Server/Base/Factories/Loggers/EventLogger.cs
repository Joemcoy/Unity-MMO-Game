using System;

using Base.Data.Enums;
using Base.Data.Interfaces;

namespace Base.Factories.Loggers
{
    public class EventLogger : ILogger
    {
        public string Name { get; private set; }

        public EventLogger(string Name)
        {
            this.Name = Name;
        }

        public void LogInfo(string Message, params object[] Arguments)
        {
            Enqueue(new Action(() => LoggerFactory.FireOnLog(this, LogType.Information, string.Format(Message, Arguments))));
        }

        public void LogDebug(string Message, params object[] Arguments)
        {
            Enqueue(new Action(() => LoggerFactory.FireOnLog(this, LogType.Debug, string.Format(Message, Arguments))));
        }

        public void LogSuccess(string Message, params object[] Arguments)
        {
            Enqueue(new Action(() => LoggerFactory.FireOnLog(this, LogType.Success, string.Format(Message, Arguments))));
        }

        //WTF?
        public void LogWarning(string Message, params object[] Arguments)
        {
            Enqueue(new Action(() => LoggerFactory.FireOnLog(this, LogType.Warning, string.Format(Message, Arguments))));
        }

        public void LogError(string Message, params object[] Arguments)
        {
            Enqueue(new Action(() => LoggerFactory.FireOnLog(this, LogType.Error, string.Format(Message, Arguments))));
        }

        public void LogFatal(Exception Error)
        {
            string Message = string.Format("{0}: {1}", Error.GetType(), Error.Message);
            Message += Environment.NewLine + Error.StackTrace;
            Enqueue(new Action(() => LoggerFactory.FireOnLog(this, LogType.Fatal, Message)));
        }

        void Enqueue(Action LogAction)
        {
            LoggerFactory.EnqueueAction(LogAction);
        }
    }
}

