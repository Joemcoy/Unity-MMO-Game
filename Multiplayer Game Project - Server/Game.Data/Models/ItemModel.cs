using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IT = Game.Data.Enums.ItemType;
using Base.Data.Interfaces;
using Network.Data.Interfaces;
using Game.Data.Abstracts;

namespace Game.Data.Models
{
    public class ItemModel : APacketWrapper, IModel
    {
        public int ID { get; set; }
        public int UniqueID { get; set; }
        public IT Type { get; set; }

        public bool CanEquip()
        {
            switch(Type)
            {
                case IT.Armor:
                case IT.Weapon:
                case IT.Secondary:
                case IT.Scroll:
                case IT.ActionScroll:
                case IT.Tool:
                    return true;
            }
            return false;
        }

        public int GetInventoryID()
        {
            switch(Type)
            {
                //case IT.Food:
                //case IT.Potion:
                    //return 1;
                case IT.Skill:
                    //return 2;
                case IT.Motion:
                    return 1;
                default:
                    return 0;
            }
        }

        public bool CanDrop()
        {
            switch(Type)
            {
                case IT.Skill:
                case IT.Motion:
                case IT.Macro:
                    return false;
            }
            return true;
        }
    }
}
