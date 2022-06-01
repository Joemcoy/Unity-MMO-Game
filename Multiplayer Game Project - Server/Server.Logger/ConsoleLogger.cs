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
    public class ConsoleLogger
    {
        public static void Fire(object Sender, LoggerEventArgs e)
        {
            Console.ResetColor();
            //Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("[{0} - ", DateTime.Now);

            switch (e.Type)
            {
                case LogType.Information:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case LogType.Success:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogType.Fatal:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
            }
            Console.Write(e.Logger.Name);
            Console.ResetColor();
            //Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("]: ");

            Console.WriteLine(e.Message);
            Console.ResetColor();
        }
    }
}
