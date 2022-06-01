using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Base.Data.Interfaces;

namespace Base.Data.EventArgs
{
    public class ThreadCallbackEventArgs : System.EventArgs
    {
        public IThread Thread { get; private set; }

        public ThreadCallbackEventArgs(IThread Thread)
        {
            this.Thread = Thread;
        }
    }
}
