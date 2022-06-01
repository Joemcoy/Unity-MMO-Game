using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Base.Data.Interfaces
{
    public interface IDispatcher<TDispatcherBase>
        where TDispatcherBase : IDispatcherBase
    {
        int Count { get; }

        void RegisterDispatcher<TDispatcher>()
            where TDispatcher : TDispatcherBase;

        void RegisterDispatcher(IDispatcherBase Dispatcher);
        void Dispatch(Action<IDispatcherBase> Method);
    }
}
