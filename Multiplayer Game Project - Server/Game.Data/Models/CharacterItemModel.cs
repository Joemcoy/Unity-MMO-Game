using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Base.Data.Interfaces;
using Game.Data.Abstracts;
using Game.Data.Attributes;
using Network.Data.Interfaces;

namespace Game.Data.Models
{
    public class CharacterItemModel : APacketWrapper, IModel
    {
        public CharacterItemModel()
        {
            HotbarSlot = -1;
            IsDirty = false;
        }

        public int ID { get; set; }
        public int OwnerID { get; set; }
        public int ItemID { get; set; }
        public bool Equiped { get; set; }
        public uint Slot { get; set; }
        public uint Amount { get; set; }
        public short HotbarSlot { get; set; }
        public Guid Serial { get; set; }

        [NonColumn]
        public bool IsDirty { get; set; }
    }
}
