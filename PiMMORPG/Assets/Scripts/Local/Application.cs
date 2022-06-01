using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using uApplication = UnityEngine.Application;

using tFramework.Factories;
using tFramework.EventArgs;
using tLogType = tFramework.Enums.LogType;

using PiMMORPG.Client;
using PiMMORPG.Client.Auth;
using PiMMORPG.Client.Interfaces;

namespace Scripts.Local
{
    using UI;
    using Inventory;
    using Control;
    using Bundles;
    using Configuration;

    public class Application : SingletonBehaviour<Application>
    {
        List<SingletonRegisterer> Registerers;

        public static GameConfiguration Configuration;
        public static PiAuthClient auth;
        public static IGameClient client;

        public const int SceneKeepMinimumMemory = 5 * 1024;
        public static bool KeepScenes { get; private set; }

        DateTime StartDate;
        void Awake()
        {
            uApplication.backgroundLoadingPriority = ThreadPriority.BelowNormal;
            StartDate = DateTime.Now;
            Registerers = new List<SingletonRegisterer>();
            //LoggerFactory.BaseType = typeof(UnityLogger);
        }

        void OnEnable()
        {
            LoggerFactory.OnLog += LogToDebug;
            //uApplication.logMessageReceived += LogToFile;
            uApplication.logMessageReceivedThreaded += LogToFile;
        }

        void OnDisable()
        {
            LoggerFactory.OnLog -= LogToDebug;
            //uApplication.logMessageReceived -= LogToFile;
            uApplication.logMessageReceivedThreaded -= LogToFile;
        }

        IEnumerator Start()
        {
            if(SystemInfo.systemMemorySize < SceneKeepMinimumMemory)
            {
                Debug.LogWarningFormat("To avoid problems with RAM, scenes will not stay in memory, this may slow down the transition of scenes. (You have {0}MB of RAM, you need {1}MB).", SystemInfo.systemMemorySize, SceneKeepMinimumMemory);
                KeepScenes = false;
            }

            yield return null;
            ClientVerifier.Instance.Run();
        }

        void OnDestroy()
        {
            if (auth != null)
                auth.Socket.Disconnect();

            if (client != null)
                client.Socket.Disconnect();

            foreach (var Registerer in Registerers)
            {
                foreach (var Singleton in Registerer.Singletons)
                    SingletonFactory.Destroy(Singleton);
            }
            Registerers.Clear();

            SingletonFactory.DestroyAll();
        }

        public void Quit()
        {
            uApplication.Quit();
        }

        public void ReturnToMenu()
        {
            PiBaseClient.Current.Socket.Disconnect();
        }

        public static void SocketDisconnected()
        {
            SafeInvoker.Create(() =>
            {
                WorldControl.RemoveDrops();
                WorldControl.RemoveTrees();
                WorldControl.RemovePlayers();
                InventoryHelper.Instance.Enabled = false;
                var cb = new Action(() =>
                 {
                     var menu = FindObjectOfType<MainMenu>();
                     menu.SwitchToMenu();
                 });

                if (BundleLoader.CurrentBScene != "menu")
                    BundleLoader.LoadScene("menu", cb);
                else
                    cb();
            });
        }

        public static void Register(SingletonRegisterer Registerer)
        {
            var Instance = SingletonFactory.GetSingleton<Application>();
            Instance.Registerers.Add(Registerer);
        }

        private void LogToDebug(object sender, LogEventArgs e)
        {
            var uT = e.Type.ToString().ToUpper();
            var formatted = string.Format("{0} - {1} - {2} - {3}", e.Time, e.Logger.Name, uT, e.Message);
            switch (e.Type)
            {
                case tLogType.Success:
                case tLogType.Information:
                    Debug.Log(formatted);
                    break;
                case tLogType.Warning:
                    Debug.LogWarning(formatted);
                    break;
                case tLogType.Error:
                case tLogType.Fatal:
                    Debug.LogError(formatted);
                    break;
            }
        }

        private void LogToFile(string logString, string stackTrace, LogType type)
        {
            var logWords = Enum.GetNames(typeof(tLogType)).Select(s => s.ToUpper());
            if (!logWords.Any(w => logString.IndexOf(w) > -1))
                logString = string.Format("{0} - {1} - UnityInternal - {2}", DateTime.Now, type.ToString().ToUpper(), logString);

            var path = Path.Combine(Environment.CurrentDirectory, "Logs");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            else
            {
                var name = string.Format("Log {0}.log", StartDate.ToString("dd-MM-yyyy_HH-mm-ss"));
                path = Path.Combine(path, name);

                using (FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write))
                using (var writer = new StreamWriter(stream))
                    writer.WriteLine(logString);

            }
        }
    }
}