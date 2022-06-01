using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Base.Data.Interfaces;
using Base.Data.Enums;

namespace Base.Data.EventArgs
{
    public class LoggerEventArgs : System.EventArgs
    {
        public ILogger Logger { get; private set; }
        public LogType Type { get; private set; }
        public string Message { get; private set; }
        public DateTime Time { get; private set; }

        public LoggerEventArgs(ILogger Logger, LogType Type, string Message)
        {
            this.Logger = Logger;
            this.Type = Type;
            this.Message = Message;
            this.Time = DateTime.Now;
        }
    }
}
