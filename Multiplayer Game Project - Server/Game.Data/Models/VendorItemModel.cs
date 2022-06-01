using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data.Abstracts;
using Base.Data.Interfaces;

namespace Game.Data.Models
{
    public class VendorItemModel : APacketWrapper, IModel
    {
        public int ID { get; set; }
        public int NPCID { get; set; }
        public int ItemID { get; set; }

        public uint Level { get; set; }
        public uint BuyPrice { get; set; }
    }
}
