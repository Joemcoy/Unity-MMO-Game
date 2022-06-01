using System;

using Base.Factories;
using Base.Data.Interfaces;
using Auth.Server;

using Server.Logger;
using Base;

namespace Auth.Service
{
    public static class Program
    {
        static void Main()
        {
            LoggerFactory.OnLog += FileLogger.Fire;
            LoggerFactory.OnLog += ConsoleLogger.Fire;

            //BaseHooks.HookAll();
            ILogger Logger = LoggerFactory.GetLogger("Global");

            Logger.LogInfo("Initalizing auth service..");
            if (ComponentFactory.Enable<AuthServer>())
            {
                Logger.LogSuccess("Server initalized sucessfully!");
            }
            else
            {
                Logger.LogWarning("Failed to initalize the server!");
            }

            ThreadFactory.WaitForAll();
        }
    }
}