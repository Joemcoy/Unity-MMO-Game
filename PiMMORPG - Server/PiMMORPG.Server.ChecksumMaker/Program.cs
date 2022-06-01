using System;
using System.Collections.Generic;
using tFramework.Enums;
using tFramework.EventArgs;
using tFramework.Factories;

namespace PiMMORPG.Server.ChecksumMaker
{
    using General;
    internal class Program
    {
        public static void Main(string[] args)
        {
            string target = string.Empty;

            foreach(var arg in args)
                Console.WriteLine(arg);
            
            if (args.Length > 0)
                target = args[0];
            else
            {
                Console.Write("Insira o diretório do jogo: ");
                target = Console.ReadLine();
            }

            while (true)
            {
                ServerControl.GenerateChecksum(target);

                Console.WriteLine("Press enter to repeat!");
                Console.ReadLine();
            }
        }
    }
}