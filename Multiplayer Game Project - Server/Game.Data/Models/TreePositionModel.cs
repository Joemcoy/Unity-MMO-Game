using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Data.Models
{
    public class TreePositionModel : PositionModel
    {
        public int Server { get; set; }
        public int TreeID { get; set; }
    }
}
