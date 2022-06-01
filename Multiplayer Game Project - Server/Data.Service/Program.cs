using System;
using System.Linq;

using Base.Data.Interfaces;

using Data.Server;
using Base.Factories;
using Server.Logger;
using Base;
using System.Diagnostics;
using Base.Manager;
using Data.Service.ArgumentReceivers;

namespace Data.Service
{
    class Program
    {
        static void Main()
        {
            LoggerFactory.OnLog += FileLogger.Fire;
            LoggerFactory.OnLog += ConsoleLogger.Fire;
            
            ArgumentManager.Parse();
            BaseHooks.HookAll();

            ILogger Logger = LoggerFactory.GetLogger("Global");

            Logger.LogInfo("Initalizing data service..");
            if (ComponentFactory.Enable<DataServer>())
            {
                Logger.LogSuccess("Server initalized sucessfully!");

                if(!DataArguments.SkipAuth)
                    BaseHooks.StartProcess("Auth.Service.exe");
            }
            else
            {
                Logger.LogWarning("Failed to initalize the server!");
            }

            ThreadFactory.WaitForAll();
        }
    }
}