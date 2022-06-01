using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Base.Data.DispatcherBases;
using Base.Data.Enums;
using Base.Data.Interfaces;
using System.IO;
using System.Reflection;
using System.Threading;
using Base.Data.EventArgs;

namespace Server.Logger
{
    public class FileLogger
    {
        static FileStream WaitForFile(string fullPath, FileMode mode)
        {
            for (int numTries = 0; numTries < 10; numTries++)
            {
                try
                {
                    FileStream fs = new FileStream(fullPath, mode);

                    fs.ReadByte();
                    fs.Seek(0, SeekOrigin.Begin);

                    return fs;
                }
                catch (IOException)
                {
                    Thread.Sleep(100);
                }
            }

            return null;
        }

        public static void Fire(object Sender, LoggerEventArgs e)
        {
            string FilePath = new string[]
            {
                Environment.CurrentDirectory,
                "Logs",
                Assembly.GetEntryAssembly().GetName().Name, string.Format("{0} - {1}.log", e.Type, DateTime.Now.ToShortDateString().Replace('/', '-'))
            }.Aggregate(Path.Combine);

            if (!Directory.Exists(Path.GetDirectoryName(FilePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(FilePath));

            string Line = string.Format("[{0} - {1} - {2}]: {3}", DateTime.Now, e.Logger.Name, e.Type, e.Message);
            using (FileStream Stream = WaitForFile(FilePath, FileMode.OpenOrCreate))
            {
                Stream.Position = Stream.Length;

                using (StreamWriter Writer = new StreamWriter(Stream, Encoding.UTF8))
                {
                    Writer.WriteLine(Line);
                }
            }
        }
    }
}