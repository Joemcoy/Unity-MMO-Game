using System;
using System.Linq;
using Base.Data.Abstracts;

namespace Server.Configuration
{
    public class GameConfiguration : XMLConfiguration
    {
        public override bool Secure { get { return true; } }
        public override string Filename
        {
            get
            {
                return "game.config";
            }
        }

        public override void WriteDefaults()
        {
            Base.Factories.LoggerFactory.GetLogger(this).LogWarning("Writing default of game configuration!");

            MaximumCharacters = 3;
            DefaultMapID = 1;
            MaximumMessageCache = 15;
            BaseExperience = 1;
            ExperienceRate = 10;
            LauncherFTP = "http://update.4fungames.com.br/Launcher/";

            InitialHealth = 1000;
            InitialStamina = 1000;
            InitialMana = 1000;
            InitialLevel = 1;
            SpeedMultiplier = 1;
            ChatDistance = 30;
            AudioDistance = 30;
        }

        public static int MaximumCharacters { get; set; }
        public static int DefaultMapID { get; set; }
        public static int MaximumMessageCache { get; set; }
        public static uint BaseExperience { get; set; }
        public static uint ExperienceRate { get; set; }
        public static string LauncherFTP { get; set; }

        public static uint InitialHealth { get; set; }
        public static uint InitialStamina { get; set; }
        public static uint InitialMana { get; set; }
        public static uint InitialLevel { get; set; }
        public static uint InitialGold { get; set; }
        public static uint InitialSilver { get; set; }
        public static uint InitialCopper { get; set; }
        public static uint InitialRuby { get; set; }
        public static float ChatDistance { get; set; }
        public static float AudioDistance { get; set; }

        public static int SpeedMultiplier { get; set; }
    }
}