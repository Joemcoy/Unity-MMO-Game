using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace tFramework.EventArgs
{
    public class EventFiredEventArgs : System.EventArgs
    {
        public Action<object> Event { get; private set; }
        public object Args { get; private set; }
        public object Sender { get; private set; }

        public EventFiredEventArgs(Action<object> Event, object args, object sender)
        {
            this.Event = Event;
            this.Args = args;
            this.Sender = sender;
        }
    }
}