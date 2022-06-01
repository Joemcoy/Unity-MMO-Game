using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data.Attributes;
using Game.Data.Information;
using Base.Data.Interfaces;
using Network.Data;
using Network.Data.Interfaces;
using Game.Data.Abstracts;

namespace Game.Data.Models
{
    public class MapModel : APacketWrapper, IModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public string SceneID { get; set; }
        public int SpawnID { get; set; }
        public PositionModel Spawn { get; set; }
    }
}