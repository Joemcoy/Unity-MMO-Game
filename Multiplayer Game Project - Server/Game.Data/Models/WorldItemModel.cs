using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Network.Data;
using Base.Data.Interfaces;
using Network.Data.Interfaces;
using Game.Data.Abstracts;

namespace Game.Data.Models
{
    public class WorldItemModel : APacketWrapper, IModel
    {
        public int ID { get; set; }
        public int ItemID { get; set; }
        public int GroupID { get; set; }

        public WorldItemGroupModel Group { get; set; }
        public ItemModel Item { get; set; }
        public PositionModel Position { get; set; }

        public WorldItemModel()
        {
            Group = new WorldItemGroupModel();
            Item = new ItemModel();
            Position = new PositionModel();
        }
    }
}
