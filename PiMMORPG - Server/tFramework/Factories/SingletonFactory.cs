using System;
using System.Collections.Generic;

namespace tFramework.Factories
{
    using Interfaces;
    using Exceptions;

    public static class SingletonFactory
    {
        private static Dictionary<int, ISingleton> _singletons = new Dictionary<int, ISingleton>();
        private static readonly object SyncLock = new object();
        private static ILogger Logger { get { return LoggerFactory.GetLogger("SingletonFactory"); } }

        public static TSingleton GetSingleton<TSingleton>() where TSingleton : ISingleton
        {
            var singleton = GetSingleton(typeof(TSingleton));
            return singleton == null ? default(TSingleton) : (TSingleton)singleton;
        }

        public static ISingleton GetSingleton(Type singletonType)
        {
            lock (SyncLock)
            {
                NotAssignableException<ISingleton>.Test(singletonType);

                int id = singletonType.GetHashCode();

                ISingleton singleton;
                if (!_singletons.TryGetValue(id, out singleton))
                {
#if UNITY_STANDALONE
                    if (typeof(UnityEngine.MonoBehaviour).IsAssignableFrom(singletonType))
                    {
                        var Object = new UnityEngine.GameObject("Singleton - " + singletonType.Name);
                        singleton = (ISingleton)Object.AddComponent(singletonType);
                    }
                    else
#endif
                    singleton = (ISingleton)Activator.CreateInstance(singletonType);
                    singleton.Created();

                    _singletons[id] = singleton;
                }
                return singleton;
            }
        }

        public static object GetSafeSingleton(Type target)
        {
            return typeof(ISingleton).IsAssignableFrom(target) ? GetSingleton(target) : Activator.CreateInstance(target);
        }

        public static TSingleton RegisterSingleton<TSingleton>(TSingleton singleton) where TSingleton : ISingleton
        {
            return (TSingleton)RegisterSingleton(typeof(TSingleton), singleton);
        }

        public static ISingleton RegisterSingleton(Type singletonType, ISingleton singleton)
        {
            NotAssignableException<ISingleton>.Test(singletonType);

            lock (SyncLock)
            {
                int id = singletonType.GetHashCode();

                ISingleton temp;
                if (_singletons.TryGetValue(id, out temp))
                    Logger.LogWarning("Singleton {0} already been registered", singletonType.Name);
                else
                {
                    singleton.Created();
                    _singletons[id] = singleton;
                }
                return singleton;
            }
        }

        public static void Destroy<TSingleton>() where TSingleton : ISingleton
        {
            Destroy(typeof(TSingleton));
        }

        public static void Destroy<TSingleton>(TSingleton instance) where TSingleton : ISingleton
        {
            Destroy<TSingleton>();
        }

        public static void Destroy(Type singletonType)
        {
            NotAssignableException<ISingleton>.Test(singletonType);
            int id = singletonType.GetHashCode();

            Destroy(id);
        }

        static void Destroy(int id)
        {
            lock (SyncLock)
            {
                ISingleton singleton;
                if (_singletons.TryGetValue(id, out singleton))
                {
                    singleton.Destroyed();
                    _singletons.Remove(id);
                }
            }
        }

        public static void DestroyAll()
        {
            lock (SyncLock)
            {
#if DEBUG
                //Logger.LogWarning("Destroying {0} singletons...", _singletons.Count);
#endif
                foreach (var singleton in _singletons.Values)
                    singleton.Destroyed();
                _singletons.Clear();

                GC.Collect(GC.MaxGeneration);
            }
        }
    }
}
