using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace tFramework.Data.Interfaces
{
    using Bases;

    public interface IContract
    {
        Type AssociatedType { get; }

        object Deserialize(SerializerElement element);
        void Serialize(SerializerElement element, object value);
    }
}
