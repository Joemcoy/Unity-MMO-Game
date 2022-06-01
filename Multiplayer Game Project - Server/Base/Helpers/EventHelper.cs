using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Base.Data.EventArgs;

namespace Base.Helpers
{
    public static class EventHelper
    {
        public static event EventHandler<SafeEventArgs> EventFired;
        volatile static object syncLock = new object();

        public static void FireEvent(EventHandler Event, object Sender, EventArgs Arguments = null)
        {
            if (Event == null)
                return;

            if (EventFired != null)
                EventFired(null, new SafeEventArgs(new Action(() => Event.Invoke(Sender, Arguments ?? EventArgs.Empty))));
            else
                Event.Invoke(Sender, Arguments ?? EventArgs.Empty);
        }

        public static void FireEvent<TArgs>(EventHandler<TArgs> Event, object Sender, TArgs Arguments = default(TArgs))
            where TArgs : EventArgs
        {
            if (Event == null)
                return;

            if (EventFired != null)
                EventFired(null, new SafeEventArgs(new Action(() => Event.Invoke(Sender, Arguments))));
            else
                Event.Invoke(Sender, Arguments);
        }

        static void Invoke<TArgs>(EventHandler<TArgs> Event, object Sender, TArgs Arguments = default(TArgs))
            where TArgs : EventArgs
        {
            Event.Invoke(Sender, Arguments);
        }
    }
}