using System;

namespace tFramework.Data.Bases
{
    public class SerializerAttribute
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public SerializerAttribute(string name)
        {
            Name = name;
            Value = string.Empty;
        }

		public SerializerAttribute(string name, string value)
		{
			Name = name;
			Value = value;
		}

        public SerializerAttribute(string name, object value) : this(name, Convert.ToString(value ?? "NULL"))
        {

        }
    }
}