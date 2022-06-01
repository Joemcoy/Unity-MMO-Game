using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace tFramework.Helper
{
    using EventArgs;
    public static class EventHelper
    {
        public static event EventHandler<EventFiredEventArgs> EventFired;

        public static void FireEvent<TArgType>(this EventHandler<TArgType> callback, TArgType args, object sender) where TArgType : System.EventArgs
        {
            if (callback != null)
                if (EventFired != null)
                    EventFired(sender, new EventFiredEventArgs((e) => callback(sender, (TArgType)e), args, sender));
                else
                    callback(sender, args);
        }
    }
}
