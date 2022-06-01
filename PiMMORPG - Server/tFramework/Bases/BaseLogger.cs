using System;

namespace tFramework.Bases
{
    using Interfaces;
    using Factories;
    using Enums;
    using tFramework.Helper;

    public class BaseLogger : ILogger
    {
        public virtual string Name { get; set; }

        public BaseLogger(string name)
        {
            this.Name = name;
        }

        public virtual void LogInfo(object value)
        {
            LogInfo((value ?? "NULL").ToString());
        }

        public virtual void LogInfo(string message, params object[] arguments)
        {
            try
            {
                if (arguments != null && arguments.Length > 0)
                    message = string.Format(message, arguments);
            }
            catch (Exception) { }
            LoggerFactory.EnqueueLog(this, LogType.Information, message);
        }

        public virtual void LogSuccess(object value)
        {
            LogSuccess((value ?? "NULL").ToString());
        }

        public virtual void LogSuccess(string message, params object[] arguments)
        {
            try
            {
                if (arguments != null && arguments.Length > 0)
                    message = string.Format(message, arguments);
            }
            catch (Exception) { }
            LoggerFactory.EnqueueLog(this, LogType.Success, message);
        }

        public virtual void LogWarning(object value)
        {
            LogWarning((value ?? "NULL").ToString());
        }

        public virtual void LogWarning(string message, params object[] arguments)
        {
            try
            {
                if (arguments != null && arguments.Length > 0)
                    message = string.Format(message, arguments);
            }
            catch (Exception) { }
            LoggerFactory.EnqueueLog(this, LogType.Warning, message);
        }

        public virtual void LogError(object value)
        {
            LogError((value ?? "NULL").ToString());
        }

        public virtual void LogError(string message, params object[] arguments)
        {
            try
            {
                if (arguments != null && arguments.Length > 0)
                    message = string.Format(message, arguments);
            }
            catch (Exception) { }
            LoggerFactory.EnqueueLog(this, LogType.Error, message);
        }

        public virtual void LogFatal(object value)
        {
            LogFatal((value ?? "NULL").ToString());
        }

        public virtual void LogFatal(string message, params object[] arguments)
        {
            try
            {
                if (arguments != null && arguments.Length > 0)
                    message = string.Format(message, arguments);
            }
            catch (Exception) { }
            LoggerFactory.EnqueueLog(this, LogType.Fatal, message);
        }

        public virtual void LogFatal(Exception ex)
        {
            var message = StringHelper.SafeGetString(() => ex.GetType().Name);
            message += ": " + StringHelper.SafeGetString(() => ex.Message);
            message += Environment.NewLine + StringHelper.SafeGetString(() => ex.StackTrace);
            message += Environment.NewLine;
            LogFatal(message);

            if (ex != null && ex.InnerException != null)
            {
                ex = ex.InnerException;
                LoggerFactory.GetLogger(ex.GetType()).LogFatal(ex);
            }
        }
    }
}