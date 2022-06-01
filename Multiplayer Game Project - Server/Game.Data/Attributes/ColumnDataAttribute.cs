using System;

namespace Game.Data.Attributes
{
    public class ColumnDataAttribute : Attribute
    {
        public ColumnDataAttribute()
        {
            Unique = false;
            MaximumLength = 0;
        }

        public bool Unique { get; set; }
        public int MaximumLength { get; set; }
        public object DefaultValue { get; set; }
        public string Name { get; set; }
    }
}