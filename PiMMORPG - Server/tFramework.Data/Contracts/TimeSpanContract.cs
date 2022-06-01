using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace tFramework.Data.Contracts
{
    using Extensions;
    using Interfaces;
    using Bases;

    public class TimeSpanContract : IContract
    {
        public Type AssociatedType { get { return typeof(TimeSpan); } }

        public object Deserialize(SerializerElement element)
        {
            var d = element.Attributes[0].Value;
            var h = element.Attributes[1].Value;
            var m = element.Attributes[2].Value;
            var s = element.Attributes[3].Value;
            return new TimeSpan(int.Parse(d), int.Parse(h), int.Parse(m), int.Parse(s));
        }

        public void Serialize(SerializerElement element, object value)
        {
            var time = (TimeSpan)value;
            element.Attributes.Add(new SerializerAttribute("Days", time.Days));
            element.Attributes.Add(new SerializerAttribute("Hours", time.Hours));
            element.Attributes.Add(new SerializerAttribute("Minutes", time.Minutes));
            element.Attributes.Add(new SerializerAttribute("Seconds", time.Seconds));
        }
    }
}
