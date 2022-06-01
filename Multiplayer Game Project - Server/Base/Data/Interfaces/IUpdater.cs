using System;

namespace Base.Data.Interfaces
{
    public interface IUpdater
    {
        int Interval { get; }

        void Start();
        void Loop();
        void End();
    }
}

