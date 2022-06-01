using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;


namespace tFramework.Data.Bases
{
    public class SerializerElement
    {
        public SerializerElement(string name)
        {
            Name = name;
            Childrens = new List<SerializerElement>();
            Attributes = new List<SerializerAttribute>();
        }
        
        public string Name { get; set; }
        public List<SerializerAttribute> Attributes { get; set; }
        public List<SerializerElement> Childrens { get; set; }
    }
}