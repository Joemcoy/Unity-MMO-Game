using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Base.Data.Interfaces;
using Game.Data.Abstracts;
using Network.Data.Interfaces;

namespace Game.Data.Models
{
    public class CharacterCurrencyModel : APacketWrapper, IModel
    {
        public int ID { get; set; }
        public uint Gold { get; set; }
        public uint Silver { get; set; }
        public uint Copper { get; set; }
        public uint Ruby { get; set; }        
    }
}
