using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Data.Models
{
    public class NPCPositionModel : PositionModel
    {
        public int Server { get; set; }
        public int NPC { get; set; }
        public bool IsVendor { get; set; }
        public bool HasDialogue { get; set; }
    }
}