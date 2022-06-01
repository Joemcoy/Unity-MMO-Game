using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Base.Factories;
using Base.Data.Interfaces;
using Game.Data.Models;

namespace Game.Controller
{
    public class LauncherFileManager
    { 
        public static LauncherFileModel[] GetFiles()
        {
            IBaseController Base = ControllerFactory.GetBaseController("launcher_files");
            return Base.GetModels<LauncherFileModel>();
        }
    }
}
