using Base.Data.Interfaces;
using Network.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Data.Abstracts;

namespace Game.Data.Models
{
    public class LauncherFileModel : APacketWrapper, IModel
    {
        public int ID { get; set; }
        public string Filename { get; set; }
        public string Hash { get; set; }
        public long Size { get; set; }
    }
}
