using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace tFramework.Factories
{
    using Bases;
    using Interfaces;
    using Exceptions;
    using Extensions;

    public class ThreadFactory : ISingleton
    {
        private Dictionary<int, BaseThread> _threadDict;

        public static IUpdater[] Updaters { get { return GetTask<BaseUpdater, IUpdater>(); } }
        public static IThread[] Threads { get { return GetTask<BaseThread, IThread>(); } }

        static TInterface[] GetTask<TTask, TInterface>() where TTask : BaseThread where TInterface : IThread
        {
            var factory = SingletonFactory.GetSingleton<ThreadFactory>();
            var updaters = factory._threadDict.Values.Where(T => T is TTask);
            return updaters.Select(T => (TInterface)(T as TTask).Thread).ToArray();
        }

        void ISingleton.Created()
        {
            _threadDict = new Dictionary<int, BaseThread>();
        }

        void ISingleton.Destroyed()
        {
            _threadDict.Values.ForEach(t => t.Stop());
            _threadDict.Clear();
        }

        public static void Start<T>() where T: IThread
        {
            Start(typeof(T));
        }

        public static void Start(Type threadType)
        {
            NotAssignableException<IThread>.Test(threadType);

            var instance = SingletonFactory.GetSafeSingleton(threadType);
            var factory = SingletonFactory.GetSingleton<ThreadFactory>();
            var id = threadType.GetHashCode();

            if (!factory._threadDict.ContainsKey(id))
                factory._threadDict[id] = CreateBase(instance as IThread);
            factory.Start(id);
        }

        public static void Start(IThread thread)
        {
            var factory = SingletonFactory.GetSingleton<ThreadFactory>();
            var id = thread.GetHashCode();

            if (!factory._threadDict.ContainsKey(id))
                factory._threadDict[id] = CreateBase(thread);
            factory.Start(id);
        }

        static BaseThread CreateBase(IThread thread)
        {
            return thread is IUpdater ? new BaseUpdater((IUpdater)thread) : new BaseThread(thread);
        }

        void Start(int id)
        {
            _threadDict[id].Start();
        }

        public static void Stop<T>() where T : IThread
        {
            Stop(typeof(T));
        }

        public static void Stop(Type threadType)
        {
            NotAssignableException<IThread>.Test(threadType);
            var factory = SingletonFactory.GetSingleton<ThreadFactory>();
            var id = threadType.GetHashCode();

            factory.Stop(id);
        }

        public static void Stop(IThread thread)
        {
            var factory = SingletonFactory.GetSingleton<ThreadFactory>();
            var id = thread.GetHashCode();

            factory.Stop(id);
        }

        void Stop(int id)
        {
            BaseThread thread;
            if(_threadDict.TryGetValue(id, out thread))
            {
                thread.Stop();
            }
        }
    }
}