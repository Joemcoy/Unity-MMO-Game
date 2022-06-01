using System;
using System.IO;
using System.Linq;

namespace tFramework.Data.Manager
{
    using Factories;
    using Helper;
    using tFramework.Interfaces;
    using Data.Interfaces;
    using Data.Serializer;

    public class ConfigurationManager
    {
        static ILogger Logger { get { return LoggerFactory.GetLogger<ConfigurationManager>(); } }
        public static string TargetDirectory { get { return Path.Combine(Environment.CurrentDirectory, "Configuration"); } }

        public static bool Load<T>(ref T configuration) where T : IConfiguration, new()
        {
            if (configuration == null) configuration = new T();
            return Load(configuration);
        }

        public static bool Load<T>(T configuration) where T : IConfiguration, new()
        {
            string fullPath = Path.Combine(TargetDirectory, configuration.Filename);

            try
            {
                if (!File.Exists(fullPath))
                    return Save(configuration);

                while (IOHelper.IsLocked(fullPath))
                {
                    Logger.LogWarning("File {0} is locked!", Path.GetFileName(fullPath));
                    System.Threading.Thread.Sleep(5000);
                }

                var fs = File.Open(fullPath, FileMode.Open);
                using (var stream = configuration.Secure ? CryptHelper.DecryptRijndael(fs) : fs)
                {
                    var result = XMLSerializer.Load(configuration, stream);
                    if (result)
                    {
                        ReflectionHelper.CallMethod(typeof(T), "Loaded", configuration);
                        ReflectionHelper.CallMethod(typeof(T), "Loaded", null, configuration);
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                Logger.LogFatal(ex);
                return Save(configuration);
            }
        }

        public static bool Save<T>(T configuration) where T : IConfiguration, new()
        {
            try
            {
                if (configuration == null) configuration = new T();
                string fullPath = Path.Combine(TargetDirectory, configuration.Filename);

                while (IOHelper.IsLocked(fullPath))
                {
                    Logger.LogWarning("File {0} is locked!", Path.GetFileName(fullPath));
                    System.Threading.Thread.Sleep(5000);
                }

                if (File.Exists(fullPath))
                    File.Delete(fullPath);
                if (!Directory.Exists(Path.GetDirectoryName(fullPath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

                using (var memory = new MemoryStream())
                {
                    if (XMLSerializer.Save(configuration, memory))
                    {
                        ReflectionHelper.CallMethod(typeof(T), "Saved", configuration);
                        ReflectionHelper.CallMethod(typeof(T), "Saved", null, configuration);

                        memory.Position = 0;

                        var fs = File.Open(fullPath, FileMode.Create);
                        using (var stream = configuration.Secure ? CryptHelper.EncryptRijndael(fs) : fs)
                        {
                            int total = 0;
                            var buffer = new byte[512];
                            do
                            {
                                total = memory.Read(buffer, 0, buffer.Length);

                                if (total > 0)
                                    stream.Write(buffer, 0, total);
                            }
                            while (total > 0);
                        }
                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogFatal(ex);
                return false;
            }
        }
    }
}