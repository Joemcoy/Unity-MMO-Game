using System;
using Base.Data.Abstracts;
using Base.Data.Interfaces;

namespace Base.Configurations
{
    public class IntervalConfiguration : XMLConfiguration
    {
        public override bool Secure { get { return true; } }
        public override string Filename { get { return "intervals.config"; } }

		public static int LoggerInterval { get; set; }
        public static int ThreadRefreshInterval { get; set; }

#if !UNITY_5
        public static int ControllerInterval { get; set; }
        public static int MessageInterval { get; set; }
        public static int ShoutInterval { get; set; }
        public static int AudioInterval { get; set; }
        public static int GodAudioInterval { get; set; }
#endif

        public override void WriteDefaults()
		{
            LoggerInterval = 150;
			ThreadRefreshInterval = 10;

#if !UNITY_5
            ControllerInterval = 10 * 60 * 1000; //10 minutes in milliseconds
            MessageInterval = 5;
            ShoutInterval = 15;
            AudioInterval = 35;
            GodAudioInterval = 24 * 60 * 60;
#endif
        }
    }
}