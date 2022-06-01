using System;
using System.Threading;

using Base.Data.Interfaces;
using Base.Configurations;
using System.Security.Permissions;

namespace Base.Factories.Tasks
{
    public class ThreadTask
    {
		Thread RealThread;

        public IThread Thread;
        public bool Running { get; private set; }

        public ThreadTask(IThread Thread)
        {
            this.Thread = Thread;
			Running = false;
        }

        public void Start()
        {
            if (!Running)
            {
                try
                {
                    RealThread = new Thread(Run);
                    RealThread.Name = string.Format("ThreadTask - {0}", Thread is UpdaterTask ? (Thread as UpdaterTask).Updater.GetType() : GetType());
                    RealThread.Start();
                    Running = true;
                }
                catch (Exception ex)
                {
                    LoggerFactory.GetLogger(this).LogFatal(ex);
                    Running = false;
                }
            }
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
        public void Stop()
        {
            if (Running)
            {
                try
                {
                    Running = false;

                    RealThread.Interrupt();

                    if (!RealThread.Join(2000))
                        RealThread.Abort();
                }
                catch (ThreadInterruptedException) { }
                catch (ThreadAbortException) { }
            }
        }

        void Run()
        {
            try
            {
                do
                {
                    Thread.Run();

                    System.Threading.Thread.Sleep(IntervalConfiguration.ThreadRefreshInterval);
                }
                while (Thread.Loop && Running);
            }
            catch (ThreadAbortException) { }
            catch (ThreadInterruptedException) { }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                LoggerFactory.GetLogger(this).LogFatal(ex);
            }
            finally
            {
                Thread.End();
                Running = false;
            }
        }
    }
}