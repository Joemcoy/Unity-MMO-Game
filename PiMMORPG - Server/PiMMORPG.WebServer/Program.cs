using System;
using System.IO;
using System.Linq;

using Nancy;

using tFramework.Enums;
using tFramework.EventArgs;
using tFramework.Factories;

namespace PiMMORPG.WebServer
{
    using Enums;
    using Server.Auth;
    using Server.RPG;
    using Server.General;
    using Server.BattleRoyale;
    using Server.RPG.Commands;
    using Server.General.Commands;
    using Server.BattleRoyale.Commands;

    internal class Program
	{
        static string StartDate, StartTime;

		public static void Main(string[] args)
		{
            var date = DateTime.Now;
            StartDate = date.ToString("dd_MM_yyyy");
            StartTime = date.ToString("HH_mm");

			LoggerFactory.OnLog += LoggerFactoryOnOnLog;
            
            ServerControl.RegisterServerType<PiRPGServer>(ServerType.RPG);
            ServerControl.RegisterServerType<PiBRServer>(ServerType.BattleRoyale);

            CommandFactory.RegisterCommands<BaseCommand>(typeof(ServerControl).Assembly);
            CommandFactory.RegisterCommands<BRCommand>(typeof(PiBRServer).Assembly);
            CommandFactory.RegisterCommands<RPGCommand>(typeof(PiRPGServer).Assembly);
#if DEBUG
            StaticConfiguration.DisableErrorTraces = false;
			StaticConfiguration.Caching.EnableRuntimeViewDiscovery = true;
			StaticConfiguration.Caching.EnableRuntimeViewUpdates = true;
#endif
            var logger = LoggerFactory.GetLogger<Program>();

            if (!ComponentFactory.Enable<ServerControl>())
                logger.LogWarning("Failed to open the server control!");
            else if (!ComponentFactory.Enable<PiAuthServer>())
				logger.LogWarning("Failed to open the auth server!"); 
            else if(!ServerControl.OpenAll())
                logger.LogWarning("Failed to open the channels!");
            else if (!ComponentFactory.Enable<WebServer>())
				logger.LogError("Failed to init the web server!");
			else
				return;

			Console.ReadLine();
			SingletonFactory.DestroyAll();
		}

		private static void LoggerFactoryOnOnLog(object sender, LogEventArgs e)
		{
			if(WebServer.Logs.Count == 20)
				WebServer.Logs.RemoveAt(0);
			WebServer.Logs.Add(e);
            WebServer.LogUpdated = DateTime.Now;
			
			Console.ResetColor();
			switch (e.Type)
			{
				case LogType.Information:
					Console.ForegroundColor = ConsoleColor.DarkCyan;
					break;
				case LogType.Success:
					Console.ForegroundColor = ConsoleColor.DarkGreen;
					break;
				case LogType.Warning:
					Console.ForegroundColor = ConsoleColor.DarkYellow;
					break;
				case LogType.Error:
					Console.ForegroundColor = ConsoleColor.DarkRed;
					break;
				case LogType.Fatal:
					Console.ForegroundColor = ConsoleColor.Red;
					break;
			}
            
			Console.Write(e.Logger.Name);
			Console.ResetColor();
			Console.WriteLine(": {0}", e.Message);

            var path = Path.Combine(Environment.CurrentDirectory, "Logs", StartDate);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            else
            {
                var name = string.Format("Log {0}.log", StartTime);
                path = Path.Combine(path, name);

                using (var stream = File.AppendText(path))
                {
                    var message = string.Format("{0} - {1} - {2} - {3}", DateTime.Now, e.Logger.Name, e.Type.ToString().ToUpper(), e.Message);
                    stream.WriteLine(message);
                }
            }
		}
	}
}