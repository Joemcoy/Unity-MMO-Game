using System;
using Base.Data.Interfaces;
using System.Threading;

namespace Base.Factories.Tasks
{
    public class UpdaterTask : IThread
    {
        public IUpdater Updater { get; private set; }
        public bool Loop { get { return true; } }
        public bool Running { get { return ThreadFactory.IsRunning(this); } }

        bool DoStop = false;

        public UpdaterTask(IUpdater Updater)
        {
            this.Updater = Updater;
        }

        public void Start()
        {
            DoStop = false;

            ThreadFactory.Start(this);
            Updater.Start();
        }

        public void Stop()
        {
            DoStop = true;
            ThreadFactory.Stop(this);
        }

        public void Run()
        {
            if(!DoStop)
                Updater.Loop();
            
            Thread.Sleep(Updater.Interval);
        }

        public void End()
        {
            Updater.End();
        }
    }
}

