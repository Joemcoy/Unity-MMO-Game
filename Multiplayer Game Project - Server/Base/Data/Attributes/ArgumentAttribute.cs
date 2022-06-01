using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base.Data.Attributes
{
    public class ArgumentAttribute : Attribute
    {
        public string Name { get; set; }

        public ArgumentAttribute(string Argument)
        {
            this.Name = Argument;
        }
    }
}
