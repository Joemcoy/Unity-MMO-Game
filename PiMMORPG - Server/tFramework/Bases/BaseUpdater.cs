using System;
using System.Linq;
using System.Text;
using SThread = System.Threading.Thread;

namespace tFramework.Bases
{
    using Interfaces;
    using Enums;

    public partial class BaseUpdater : BaseThread
    {
        IUpdater _updater;

        public BaseUpdater(IUpdater updater) : base(updater)
        {
            this._updater = updater;
        }

        public override bool Run()
        {
            if (_updater.DelayMode == DelayMode.DelayBefore)
                SThread.Sleep(_updater.Interval);

            if (base.Run())
            {
                if (_updater.DelayMode == DelayMode.DelayAfter)
                    SThread.Sleep(_updater.Interval);
                return true;
            }
            else
                return false;
        }
    }
}