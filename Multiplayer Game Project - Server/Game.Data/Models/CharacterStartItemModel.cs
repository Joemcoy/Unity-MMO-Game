using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base.Data.Interfaces;

namespace Game.Data.Models
{
    public class CharacterStartItemModel : IModel
    {
        public int ID { get; set; }
        public int Class { get; set; }
        public int ItemID { get; set; }
        public uint Amount { get; set; }
    }
}
