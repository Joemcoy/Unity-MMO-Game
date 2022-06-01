////#define MONO

using System;

using Game.Data;
using Base.Factories;
using Base.Data.Interfaces;
using Base.Data.Enums;
using Base.Data.EventArgs;
using Launcher.Server;

using Server.Logger;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Game.Data.Information;
using Server.Configuration;

namespace Launcher.Service
{
    public static class Program
    {
#if !MONO
        private delegate bool ConsoleEventDelegate(int eventType);
        static ConsoleEventDelegate handler;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);
#endif

        static void Main()
        {
#if !MONO
            handler = new ConsoleEventDelegate(OnCloseCallback);
            SetConsoleCtrlHandler(handler, true);
#endif

            LoggerEvents.InitTime = DateTime.Now;
            Console.Clear(); LoggerFactory.LoggerCallback += LoggerEvents.FileLog;
            LoggerFactory.LoggerCallback += LoggerEvents.ConsoleLog;

            ILogger Logger = LoggerFactory.GetLogger("Global");

            Logger.LogInfo($"Initalizing launcher service..");
            if (ComponentFactory.Enable<LauncherServer>())
            {
                Logger.LogSuccess("Server initalized sucessfully!");
            }
            else
            {
                Logger.LogWarning($"Failed to initalize the server!");
                Console.ReadLine();
            }
            Console.ReadLine();
        }

        private static bool OnCloseCallback(int eventType)
        {
            if (eventType == 2)
            {
                LoggerFactory.GetLogger().LogInfo("Destroying/Saving all data...");
                SingletonFactory.DestroyAll();
            }
            return false;
        }
    }
}