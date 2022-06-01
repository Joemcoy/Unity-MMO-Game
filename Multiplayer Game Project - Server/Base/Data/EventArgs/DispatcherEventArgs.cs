using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Base.Data.Interfaces;
using Base.Data.Abstracts;

namespace Base.Data.EventArgs
{
    public class DispatcherEventArgs<TDispatcherBase> : System.EventArgs
        where TDispatcherBase : IDispatcherBase
    {
        public TDispatcherBase Base { get; private set; }
        public ADispatcher<TDispatcherBase> Dispatcher { get; private set; }
        public Action<TDispatcherBase> Method { get; private set; }

        public DispatcherEventArgs(TDispatcherBase Base, ADispatcher<TDispatcherBase> Dispatcher, Action<TDispatcherBase> Method)
        {
            this.Base = Base;
            this.Dispatcher = Dispatcher;
            this.Method = Method;
        }
    }
}
