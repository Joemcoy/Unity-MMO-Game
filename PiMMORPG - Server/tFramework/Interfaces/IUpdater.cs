using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tFramework.Interfaces
{
    using Enums;

    public interface IUpdater : IThread
    {
        int Interval { get; }
        DelayMode DelayMode { get; }
    }
}
