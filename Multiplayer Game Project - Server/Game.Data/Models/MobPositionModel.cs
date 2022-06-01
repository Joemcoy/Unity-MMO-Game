using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Data.Models
{
    public class MobPositionModel : PositionModel
    {
        public int MobID { get; set; }
        public int Server { get; set; }
    }
}
