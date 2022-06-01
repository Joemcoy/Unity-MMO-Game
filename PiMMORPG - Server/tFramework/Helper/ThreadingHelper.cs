#if !UNITY_EDITOR && !UNITY_STANDALONE && !UNITY_ANDROID && !UNITY_MOBILE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tFramework.Helper
{
    public class ThreadingHelper
    {
        public static void RunSync(Task t)
        {
            var task = Task.Run(async () => await t);
            task.Wait();
        }

        public static T RunSync<T>(Task<T> t)
        {
            var task = Task.Run(async () => await t);
            task.Wait();

            return task.Result;
        }
    }
}
#endif