using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Nancy.Hosting.Self;
using tFramework.Data.Manager;
using tFramework.EventArgs;
using tFramework.Factories;
using tFramework.Interfaces;

namespace PiMMORPG.WebServer
{
    public class WebServer : IComponent, ISingleton
    {
        private NancyHost _host;
        private HostConfiguration _config;
        private ILogger _logger;

        public static WebConfiguration Configuration;
        public static Bootstraper Bootstraper { get; private set; }
        public static List<LogEventArgs> Logs { get; private set; } = new List<LogEventArgs>();
        public static DateTime LogUpdated { get; set; } = DateTime.Now;

        public static DriveInfo Drive
        {
            get
            {
                return DriveInfo.GetDrives().FirstOrDefault(d => d.Name == Path.GetPathRoot(Environment.CurrentDirectory));
            }
        }

        public static uint TotalDisk { get { return Convert.ToUInt32(Round((ulong)Drive.TotalSize, 0)); } }
        public static double FreeDisk { get { return Round((ulong)(Drive.TotalSize - Drive.TotalFreeSpace), 2); } }

        static double Round(ulong Value, int r)
        {
            return Math.Round(Convert.ToDouble(Value) / 1024 / 1024 / 1024, r);
        }

        void ISingleton.Created()
        {
            _logger = LoggerFactory.GetLogger(this);
            Bootstraper = new Bootstraper();
        }

        void ISingleton.Destroyed()
        {

        }

        bool IComponent.Enable()
        {
            try
            {
                if (ConfigurationManager.Load(ref Configuration) && UserDatabase.LoadSessions() && APIDatabase.LoadSessions())
                {
                    if (_config == null)
                    {
                        _config = new HostConfiguration();
                        _config.RewriteLocalhost = false;
                        _config.UrlReservations.CreateAutomatically = true;
                        _config.UnhandledExceptionCallback += e => _logger.LogFatal(e);
                    }

                    if (_host == null)
                        _host = new NancyHost(new Uri(Configuration.Url), new Bootstraper(), _config);
                    _host.Start();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogFatal(ex);
            }
            return false;
        }

        bool IComponent.Disable()
        {
            try
            {
                if (_host != null)
                {
                    _host.Stop();
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogFatal(ex);
            }
            return false;
        }
    }
}