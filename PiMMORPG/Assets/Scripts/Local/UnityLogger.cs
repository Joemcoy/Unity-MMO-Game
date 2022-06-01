using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using tFramework.Bases;

namespace Scripts.Local
{
    public class UnityLogger : BaseLogger
    {
        public UnityLogger(string Name) : base(Name) { }

        public override void LogInfo(object Value)
        {
            //base.LogInfo(Value);
            Debug.Log(Value);
        }

        public override void LogInfo(string Message, params object[] Arguments)
        {
            //base.LogInfo(Message, Arguments);
            Debug.LogFormat(Message, Arguments);
        }

        public override void LogSuccess(object Value)
        {
            //base.LogSuccess(Value);
            Debug.Log(Value);
        }

        public override void LogSuccess(string Message, params object[] Arguments)
        {
            //base.LogSuccess(Message, Arguments);
            Debug.LogFormat(Message, Arguments);
        }

        public override void LogWarning(object Value)
        {
            //base.LogWarning(Value);
            Debug.LogWarning(Value);
        }

        public override void LogWarning(string Message, params object[] Arguments)
        {
            //base.LogWarning(Message, Arguments);
            Debug.LogWarningFormat(Message, Arguments);
        }

        public override void LogError(object Value)
        {
            //base.LogError(Value);
            Debug.LogError(Value);
        }

        public override void LogError(string Message, params object[] Arguments)
        {
            //base.LogError(Message, Arguments);
            Debug.LogErrorFormat(Message, Arguments);
        }

        public override void LogFatal(object value)
        {
            //base.LogFatal(value);
            LogError(value);
        }

        public override void LogFatal(string message, params object[] arguments)
        {
            //base.LogFatal(message, arguments);
            LogError(message, arguments);
        }

        public override void LogFatal(Exception ex)
        {
            //base.LogFatal(ex);
            Debug.LogException(ex);
        }
    }
}
