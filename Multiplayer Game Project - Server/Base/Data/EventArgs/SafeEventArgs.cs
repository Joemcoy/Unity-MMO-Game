using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base.Data.EventArgs
{
    public class SafeEventArgs : System.EventArgs
    {
        public Action Action { get; private set; }

        public SafeEventArgs(Action SafeAction)
        {
            this.Action = SafeAction;
        }
    }
}
