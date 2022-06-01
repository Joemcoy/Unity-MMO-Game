using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using tFramework.Extensions;
using tFramework.Data.Interfaces;

namespace Scripts.Local.Configuration
{
    using Control;

    /*public class VideoConfiguration
    {
        public bool FullScreen { get; set; }
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
        public int RefreshRate { get; set; }
        public int QualityLevel { get; set; }
    }*/

    public class GraphicsConfiguration
    {
        public bool AntiAliasing { get; set; }
        public bool Bloom { get; set; }
        public bool Fog { get; set; }
        public bool AmbientOcclusion { get; set; }
        public bool DepthOfField { get; set; }
        public bool MotionBlur { get; set; }
        public bool ColorGrading { get; set; }
        public bool ChromaticAberration { get; set; }
        public bool UserLut { get; set; }
        public bool EyeAdaption { get; set; }
        public bool ScreenSpaceReflection { get; set; }
    }

    public class NetworkConfiguration
    {
        public string ServerAddress { get; set; }
        public string LastUsername { get; set; }
    }

    public class GameConfiguration : IConfiguration
    {
        string IConfiguration.Filename { get { return "game.config"; } }
        bool IConfiguration.Secure { get { return false; } }

        //public VideoConfiguration Video { get; set; }
        public GraphicsConfiguration Graphics { get; set; }
        public NetworkConfiguration Network { get; set; }

        public GameConfiguration()
        {
            /*Video = new VideoConfiguration();
            Video.FullScreen = Screen.fullScreen;
            Video.ScreenWidth = Screen.currentResolution.width;
            Video.ScreenHeight = Screen.currentResolution.height;
            Video.RefreshRate = Screen.currentResolution.refreshRate;
            Video.QualityLevel = QualitySettings.GetQualityLevel();*/

            Graphics = new GraphicsConfiguration();

            Network = new NetworkConfiguration();
            Network.LastUsername = string.Empty;
            Network.ServerAddress = "americas1.4fun.games:1793";
        }

        void Loaded()
        {
            //Screen.SetResolution(Video.ScreenWidth, Video.ScreenHeight, Video.FullScreen, Video.RefreshRate);
            //QualitySettings.SetQualityLevel(Video.QualityLevel, true);

            Camera.allCameras.ForEach(c => WorldControl.LoadEffects(c.gameObject));
        }

        void Saved()
        {
            Loaded();
        }
    }
}
