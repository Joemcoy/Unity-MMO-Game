using System;

namespace Base.Data.Exceptions
{
    public class NotImplementedInterfaceException : Exception
    {
        public NotImplementedInterfaceException(Type Target, Type Interface)
            : base(string.Format("The class {0} don't implement {1} type!", Target, Interface))
        {
        }
    }
}

