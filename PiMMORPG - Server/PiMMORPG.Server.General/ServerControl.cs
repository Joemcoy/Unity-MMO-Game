using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using tFramework.Factories;
using tFramework.Interfaces;

using tFramework.Helper;
using tFramework.Data.Manager;
using tFramework.Data.Serializer;
using tFramework.DataDriver.Interfaces;

namespace PiMMORPG.Server.General
{
    using Enums;
    using Models;
    using Drivers;
    using Interfaces;

    public class ServerControl : ISingleton, IComponent
    {
        public static IGameServer[] Servers
        {
            get
            {
                var control = SingletonFactory.GetSingleton<ServerControl>();
                return control.servers.Values.ToArray();
            }
        }
        public static ServerConfiguration Configuration = null;

        static ILogger logger;
        Dictionary<int, IGameServer> servers;
        Dictionary<ServerType, Type> serverTypes;

        void ISingleton.Created()
        {
            servers = new Dictionary<int, IGameServer>();
            serverTypes = new Dictionary<ServerType, Type>();
            logger = LoggerFactory.GetLogger(this);
        }

        void ISingleton.Destroyed()
        {
            foreach (var server in servers.Values)
                ComponentFactory.Disable(server);
            servers.Clear();
            serverTypes.Clear();
        }

        bool IComponent.Enable()
        {
            if (!ConfigurationManager.Load(ref Configuration))
                return false;
            else
            {
                logger.LogInfo("Preparing drivers...");
                try
                {
                    foreach (var type in typeof(ChannelDriver).Assembly.GetTypes())
                    {
                        if (!type.IsAbstract && !type.IsInterface && typeof(IDriver).IsAssignableFrom(type))
                        {
                            logger.LogInfo("Preparing driver {0}...", type.Name);
                            var ctx = Activator.CreateInstance(type) as IDriver;
                            ctx.Dispose();
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogFatal(ex);
                    return false;
                }

                using (var ctx = new ChannelDriver())
                {
                    foreach (var channel in ctx.GetModels())
                    {
                        if(!RegisterServer(channel, false))
                        {
                            logger.LogError("Failed to register the server {0}:{1}!", channel.Type, channel.Port);
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        bool IComponent.Disable()
        {
            foreach (var server in servers.Values)
                if (!ComponentFactory.Disable(server))
                    return false;
            return true;
        }

        public static bool OpenAll()
        {
            var control = SingletonFactory.GetSingleton<ServerControl>();
            foreach (var server in control.servers.Values)
                if (!ComponentFactory.Enable(server))
                    return false;
            return true;
        }

        public static void RegisterServerType<TServer>(ServerType type) where TServer : IGameServer
        {
            var control = SingletonFactory.GetSingleton<ServerControl>();
            control.serverTypes[type] = typeof(TServer);
        }

        public static bool RegisterServer(Channel channel, bool open = true)
        {
            var control = SingletonFactory.GetSingleton<ServerControl>();
            var key = channel.Port;
            Type type = null;

            if (control.servers.ContainsKey(key) || !control.serverTypes.TryGetValue(channel.Type, out type))
                return false;
            else
            {
                var server = Activator.CreateInstance(type) as IGameServer;
                server.Channel = channel;

                if (open && !ComponentFactory.Enable(server))
                    return false;
                else
                {
                    control.servers[key] = server;
                    return true;
                }
            }
        }

        public static IGameServer GetServer(int Port)
        {
            var control = SingletonFactory.GetSingleton<ServerControl>();
            IGameServer server;

            return control.servers.TryGetValue(Port, out server) ? server : null;
        }

        static void CreateHash(string hashPath, string hash)
        {
            if (File.Exists(hashPath)) File.Delete(hashPath);

            var codedHash = Encoding.UTF8.GetBytes(hash).Select(b => Convert.ToByte(b <= 0xFF - 0x5 ? b + 0x5 : b)).ToArray();
            using (var fs = CryptHelper.EncryptRijndael(File.Create(hashPath)))
                fs.Write(codedHash, 0, codedHash.Length);
        }

        public static void GenerateChecksum(string directoryPath)
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, "Checksum.xml");
            if (File.Exists(filePath))
                File.Delete(filePath);

            var localHash = Path.Combine(directoryPath, "Checksum.xml");
            if (File.Exists(localHash))
                File.Delete(localHash);

            /*var hashes = Directory.GetFiles(directoryPath, "*.hash", SearchOption.AllDirectories);
            foreach (var hashFile in hashes)
                File.Delete(hashFile);*/

            var excluded = new[] { "Configuration", "Logs" };

            var files = Directory.GetDirectories(directoryPath, "*.*", SearchOption.AllDirectories)
                .Where(d => !excluded.Any(e => e == Path.GetFileName(d)))
                .SelectMany(d => Directory.GetFiles(d))
                .Concat(Directory.GetFiles(directoryPath, "*.*", SearchOption.TopDirectoryOnly))
                .Where(f => !f.EndsWith(".hash"))
                .ToArray();
            if (files.Length > 0)
            {
                using (var stream = CryptHelper.EncryptRijndael(File.Create(filePath)))
                {
                    var list = new FileData[files.Length * 2];
                    for (int i = 0; i < files.Length; i++)
                    {
                        int index = i * 2;
                        string filename = files[i];

                        Console.WriteLine("Creating hash for file {0}...", filename.Replace(directoryPath + Path.DirectorySeparatorChar, string.Empty));

                        var data = list[index] = new FileData
                        {
                            FilePath = filename.Replace(directoryPath, string.Empty),
                            Hash = HashHelper.CalculateFileMD5(filename),
                            Size = new FileInfo(filename).Length
                        };

                        if (data.FilePath[0] == Path.DirectorySeparatorChar)
                            data.FilePath = data.FilePath.Substring(1);

                        var hashPath = filename + ".hash";

                        if (File.Exists(hashPath))
                        {
                            var decoded = File.ReadAllBytes(hashPath).Select(b => Convert.ToByte(b >= 5 ? b - 5 : b)).ToArray();
                            if (list[index].Hash != Encoding.UTF8.GetString(decoded))
                                CreateHash(hashPath, list[index].Hash);
                        }
                        else
                            CreateHash(hashPath, list[index].Hash);

                        var hashData = list[index + 1] = new FileData
                        {
                            FilePath = hashPath.Replace(directoryPath, string.Empty),
                            Hash = HashHelper.CalculateFileMD5(hashPath),
                            Size = new FileInfo(hashPath).Length
                        };

                        if (hashData.FilePath[0] == Path.DirectorySeparatorChar)
                            hashData.FilePath = hashData.FilePath.Substring(1);
                    }
                    list = list.OrderByDescending(f => Path.GetExtension(f.FilePath) == ".hash").ThenByDescending(f => f.Size).ToArray();

                    var comment = string.Format("File generated at {0}", DateTime.Now) + Environment.NewLine;
                    comment += string.Format("Total of {0} files inspected!", list.Length);

                    if (!XMLSerializer.Save(list, stream, comment))
                    {
                        Console.WriteLine("Failed to write the Checksum file!");
                    }
                }

                var hash = HashHelper.CalculateFileMD5(filePath);
                if (ConfigurationManager.Load(ref Configuration))
                {
                    Configuration.ChecksumMD5 = hash;
                    if (!ConfigurationManager.Save(Configuration))
                        Console.WriteLine("Failed to save the server configuration!");
                }
                else
                    Console.WriteLine("Failed to write the Checksum MD5 to server configuration!");
            }
        }
    }
}