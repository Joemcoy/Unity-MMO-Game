using System;

namespace tFramework.EventArgs
{
	using Enums;
	using Interfaces;

	public class LogEventArgs : System.EventArgs
	{
		public ILogger Logger { get; private set; }
		public LogType Type { get; private set; }
		public DateTime Time { get; private set; }
		public string Message { get; private set; }

		public LogEventArgs(ILogger logger, LogType type, string message)
		{
			Logger = logger;
			Type = type;
			Message = message;
			Time = DateTime.Now;
		}
	}
}