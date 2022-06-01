//#define MONO

using System;
using System.IO;
using System.Xml;

using Game.Data;
using Base.Factories;
using Base.Data.Interfaces;
using Base.Data.Enums;
using Chat.Server;

using Server.Logger;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Game.Data.Information;
using Server.Configuration;

namespace Chat.Service
{
    public static class Program
    {
#if !MONO
		private delegate bool ConsoleEventDelegate (int eventType);
		static ConsoleEventDelegate handler;

		[DllImport ("kernel32.dll", SetLastError = true)]
		private static extern bool SetConsoleCtrlHandler (ConsoleEventDelegate callback, bool add);
#endif

		static void Main ()
		{
#if !MONO
			handler = new ConsoleEventDelegate (OnCloseCallback);
			SetConsoleCtrlHandler (handler, true);
#endif

			
            Console.Clear();
            LoggerFactory.OnLog += FileLogger.Fire;
            LoggerFactory.OnLog += ConsoleLogger.Fire;

            ILogger Logger = LoggerFactory.GetLogger("Global");

            Logger.LogInfo("Initalizing chat service..");
            if (ComponentFactory.Enable<ChatServer>())
            {
                Logger.LogSuccess("Server initalized sucessfully!");                
            }
            else
            {
                Logger.LogWarning("Failed to initalize the server!");
                Console.ReadLine();
            }
            Console.ReadLine();
        }

        private static bool OnCloseCallback(int eventType)
        {
            if (eventType == 2)
            {
                try
                {
                    LoggerFactory.GetLogger("Chat Service").LogInfo("Destroying/Saving all data...");
                    SingletonFactory.DestroyAll();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            return false;
        }
    }
}