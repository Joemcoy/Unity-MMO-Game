using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Game.Data.Attributes;
using Game.Data.Information;
using Base.Data.Interfaces;
using Network.Data;
using Network.Data.Interfaces;
using Game.Data.Abstracts;
using Game.Data.Enums;

namespace Game.Data.Models
{
    public class CharacterModel : APacketWrapper, IModel
    {
        public int ID { get; set; }
        public int AID { get; set; }
        public string Name { get; set; }
        public bool IsFemale { get; set; }
        public int Class { get; set; }
        public bool Combat { get; set; }
        public int CurrentSlot { get; set; }
        public long LastPing { get; set; }

        public PositionModel Position { get; set; }
        public CharacterStatsModel Stats { get; set; }
        public CharacterStyleModel Style { get; set; }
        public CharacterCurrencyModel Currency { get; set; }
        public CharacterType Type { get { return (CharacterType)Class; } }

        public CharacterModel()
        {
            Position = new PositionModel();
            Stats = new CharacterStatsModel();
            Style = new CharacterStyleModel();
            Currency = new CharacterCurrencyModel();
        }
    }
}