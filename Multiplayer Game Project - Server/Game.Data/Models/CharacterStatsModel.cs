using Base.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Network.Data;
using Game.Data.Attributes;
using Network.Data.Interfaces;
using Game.Data.Abstracts;

namespace Game.Data.Models
{
    [System.Serializable]
    public class CharacterStatsModel : APacketWrapper, IModel
    {
        public int ID { get; set; }
        public uint Health { get; set; }
        public uint MaxHealth { get; set; }
        public uint Stamina { get; set; }
        public uint Mana { get; set; }
        public uint Level { get; set; }        
        public uint Experience { get; set; }
        public float SpeedMultipler { get; set; }
    }
}