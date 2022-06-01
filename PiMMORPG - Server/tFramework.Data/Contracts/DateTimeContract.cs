using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tFramework.Data.Contracts
{
    using Bases;
    using Extensions;
    using Interfaces;

    public class DateTimeContract : IContract
    {
        public Type AssociatedType { get { return typeof(DateTime); } }

        public object Deserialize(SerializerElement element)
        {
            /*var h = element.Attributes[0].Value;
            var m = element.Attributes[1].Value;
            var s = element.Attributes[2].Value;*/


            var d = element.Attributes[0].Value.Split('-');
            var D = d[0];
            var M = d[1];
            var y = d[2];

            var t = element.Attributes[1].Value.Split(':');
            var h = t[0];
            var m = t[1];
            var s = t[2];

            return new DateTime(int.Parse(y), int.Parse(M), int.Parse(D), int.Parse(h), int.Parse(m), int.Parse(s));
        }

        public void Serialize(SerializerElement element, object value)
        {
            var date = (DateTime)value;

            element.Attributes.Add(new SerializerAttribute("Date", "{0}-{1}-{2}".Format(date.Day, date.Month, date.Year)));
            element.Attributes.Add(new SerializerAttribute("Time", "{0}:{1}:{2}".Format(date.Hour, date.Minute, date.Year)));
        }
    }
}