using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Base.Data.Interfaces;
using Game.Data.Abstracts;

namespace Game.Data.Models
{
    public class TreeModel : APacketWrapper, IModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
