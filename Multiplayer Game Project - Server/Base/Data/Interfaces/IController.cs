#if !(UNITY_5)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base.Data.Interfaces
{
    public interface IController
    {
        Type ModelType { get; }
        string TableName { get; }
    }
}
#endif