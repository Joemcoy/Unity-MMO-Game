using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data.Attributes;
using Game.Data.Information;
using Base.Data.Interfaces;
using Game.Data.Abstracts;

namespace Game.Data.Models
{
    public class SpawnModel : APacketWrapper, IModel
    {
        public int ID { get; set; }
        public int MapID { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }

        public float RotationX { get; set; }
        public float RotationY { get; set; }
        public float RotationZ { get; set; }
        public float RotationW { get; set; }
    }
}