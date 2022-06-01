//#define MONO

using System;
using Base.Data.Interfaces;
using Base.Factories;
using Base;

using Server.Logger;
using Game.Server;
using System.Windows.Forms;

namespace Game.Service
{
    static class Program
    {
		static void Main ()
		{
            Console.Clear();
            //BaseHooks.HookAll();

            LoggerFactory.OnLog += FileLogger.Fire;
            LoggerFactory.OnLog += ConsoleLogger.Fire;

            ILogger Logger = LoggerFactory.GetLogger("Global");

            Logger.LogInfo("Initalizing game service..");
            if (ComponentFactory.Enable<GameServer>())
            {
                Logger.LogSuccess("Server initalized sucessfully!");
            }
            else
            {
                Logger.LogWarning("Failed to initalize the server!");
            }
            
            Console.ReadLine();
        }
    }
}