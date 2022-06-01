using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Base.Data.Interfaces;

namespace Base.Data.DispatcherBases
{
    public interface ISingletonDispatcher : IDispatcherBase
    {
        void OnCreate(out ISingleton Singleton);
        void OnDestroy(ISingleton Singleton);
        void OnRegister(ISingleton Singleton);
    }
}
