#if !UNITY_5
using Base.Data.Interfaces;
using Base.Factories;
using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Base
{
    public static class BaseHooks
    {
        private delegate bool ConsoleEventDelegate(int eventType);
        private delegate bool SetConsoleCtrlHandlerDelegate(ConsoleEventDelegate callback, bool add);
        
        static volatile ILogger logger;

        static ILogger Logger
        {
            get
            {
                if (logger == null)
                    logger = LoggerFactory.GetLogger("BaseHook");
                return logger;
            }
        }

        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll")]
        public static extern bool FreeLibrary(IntPtr hModule);

        public enum Platform
        {
            Windows,
            Linux,
            Mac
        }

        public static Platform RunningPlatform()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    if (Directory.Exists("/Applications") && Directory.Exists("/System") && Directory.Exists("/Users") && Directory.Exists("/Volumes"))
                        return Platform.Mac;
                    else
                        return Platform.Linux;

                case PlatformID.MacOSX:
                    return Platform.Mac;

                default:
                    return Platform.Windows;
            }
        }

        public static void StartProcess(string Executable, string ArgumentLine = "")
        {
            var Platform = RunningPlatform();
            switch(Platform)
            {
                case Platform.Linux:
                    Process.Start("gnome-terminal", string.Format("-x mono {0} {1}", Executable, ArgumentLine));
                    break;
                case Platform.Windows:
                    Process.Start(Executable, ArgumentLine);
                    break;
            }
        }

        public static void HookAll()
        {
            var Platform = (int)Environment.OSVersion.Platform;
            var Unix = Platform == 4 || Platform == 6 || Platform == 128;

            Logger.LogInfo("Trying to hook the handler...");
            if (!Unix)
            {
                /*var Handle = LoadLibrary("kernel32.dll");
                var MethodHandle = GetProcAddress(Handle, "SetConsoleCtrlHandler");
                var Method = (SetConsoleCtrlHandlerDelegate)Marshal.GetDelegateForFunctionPointer(MethodHandle, typeof(SetConsoleCtrlHandlerDelegate));*

                Method(new ConsoleEventDelegate(OnCloseCallback), true);
                Logger.LogSuccess("Handler has been hooked successfully (FreeResult: {0})!", FreeLibrary(Handle));*/
            }
            else
                Logger.LogWarning("Incompatible system....");
        }

        private static bool OnCloseCallback(int eventType)
        {
            if (eventType == 2)
            {
                try
                {
                    Logger.LogInfo("Saving all data and destroying singletons...");
                    SingletonFactory.DestroyAll();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("FATAL ERROR: {0}", ex);
                }
            }
            return false;
        }
    }
}
#endif