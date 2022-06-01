using System;
namespace tFramework.Interfaces
{
	public interface ILogger
	{
		string Name { get; }

		void LogInfo(object value);
		void LogInfo(string message, params object[] arguments);

		void LogSuccess(object value);
		void LogSuccess(string message, params object[] arguments);

		void LogWarning(object value);
		void LogWarning(string message, params object[] arguments);

		void LogError(object value);
		void LogError(string message, params object[] arguments);

        void LogFatal(object value);
        void LogFatal(string message, params object[] arguments);
        void LogFatal(Exception ex);
	}
}