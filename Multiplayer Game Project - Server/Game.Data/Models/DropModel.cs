using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Base.Data.Interfaces;
using Game.Data.Abstracts;

namespace Game.Data.Models
{
    public class DropModel : APacketWrapper, IModel
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public Guid Serial { get; set; }
        public uint Amount { get; set; }

        public PositionModel Position { get; set; }
        public DropModel() { Position = new PositionModel(); }
    }
}
