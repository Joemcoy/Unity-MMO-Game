using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data.Abstracts;
using Base.Data.Interfaces;

namespace Game.Data.Models
{
    public class MobModel : APacketWrapper, IModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool Passive { get; set; }
    }
}
