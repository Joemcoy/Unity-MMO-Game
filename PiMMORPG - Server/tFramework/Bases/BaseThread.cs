using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace tFramework.Bases
{
    using Interfaces;
    public partial class BaseThread
    {
        internal IThread Thread { get; private set; }
        private Thread _realThread;
        private bool _ended;

        public BaseThread(IThread thread)
        {
            this.Thread = thread;
        }

        public virtual void Start()
        {
            if (_realThread == null || _realThread.ThreadState != ThreadState.Running)
            {
                Thread.Start();

                _ended = false;
                _realThread = new Thread(BaseRun);
                _realThread.Start();
            }
        }

        void BaseRun()
        {
            try
            {
                while (!_ended && Run())
                    System.Threading.Thread.Sleep(10);
            }
            catch (ThreadAbortException) { }
            catch (ThreadInterruptedException) { }
            finally
            {
                CallEnd();
            }
        }

        public virtual bool Run()
        {
            try
            {
                return Thread.Run();
            }
            catch (ThreadAbortException) { }
            catch (ThreadInterruptedException) { }

            CallEnd();
            return false;
        }

        public virtual void Stop()
        {
            CallEnd();
            try
            {
                if (_realThread != null && !_realThread.Join(1000))
                    _realThread.Interrupt();
            }
            catch (ThreadAbortException) { }
            catch (ThreadInterruptedException) { }
        }

        private void CallEnd()
        {
            if (!_ended)
            {
                _ended = true;
                Thread.End();
            }
        }
    }
}