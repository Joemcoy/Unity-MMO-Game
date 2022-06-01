using System;
using System.Collections.Generic;

using Base.Data.EventArgs;
using Base.Data.Interfaces;

namespace Base.Data.Abstracts
{
    public abstract class ADispatcher<TDispatcherBase> : IDispatcher<TDispatcherBase>
        where TDispatcherBase : IDispatcherBase
    {
        public static event EventHandler<DispatcherEventArgs<TDispatcherBase>> Dispatched;

        List<TDispatcherBase> Dispatchers = new List<TDispatcherBase>();
        object syncLock = new object();

        public int Count { get { return Dispatchers.Count; } }

        public void RegisterDispatcher<TDispatcher>()
            where TDispatcher : TDispatcherBase
        {
            if (typeof(TDispatcherBase).IsAssignableFrom(typeof(TDispatcher)))
            {
                var Dispatcher = Activator.CreateInstance<TDispatcher>();
                RegisterDispatcher(Dispatcher);
            }
            else
                throw new InvalidOperationException();
        }

        public void RegisterDispatcher(TDispatcherBase Dispatcher)
        {
            lock (syncLock)
            {
                if (!Dispatchers.Contains(Dispatcher))
                {
                    Dispatchers.Add(Dispatcher);
                }
            }
        }

        public void DispatchBase(Action<TDispatcherBase> Method)
        {
            foreach (var Dispatcher in Dispatchers)
            {
                if (Dispatched != null)
                    Dispatched(this, new DispatcherEventArgs<TDispatcherBase>(Dispatcher, this, Method));
                else
                    Method.Invoke(Dispatcher);
            }
        }

        public void RegisterDispatcher(IDispatcherBase Dispatcher)
        {
            if (Dispatcher is TDispatcherBase)
                RegisterDispatcher((TDispatcherBase)Dispatcher);
            else
                throw new InvalidOperationException();
        }

        public void Dispatch(Action<IDispatcherBase> Method)
        {
            DispatchBase(d => Method(d));
        }
    }
}