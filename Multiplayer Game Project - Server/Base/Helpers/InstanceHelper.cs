using System;

using Base.Data.Interfaces;
using Base.Factories;

namespace Base.Helpers
{
    public static class InstanceHelper
    {
        static volatile object syncLock = new object();

        public static Type GetInstance<Type>()
        {
            var Obj = GetInstance(typeof(Type), true);
            return Obj == null ? default(Type) : (Type)Obj;
        }

        public static object GetInstance(Type OType)
        {
            return GetInstance(OType, true);
        }

        public static object GetInstance(Type OType, bool CallFactory)
        {
            //lock (syncLock)
            //{
#if UNITY_5
                if (typeof(UnityEngine.MonoBehaviour).IsAssignableFrom(OType))
                {
                    var IsSingleton = typeof(ISingleton).IsAssignableFrom(OType);
                    if (IsSingleton && CallFactory)
                    {
                        var Singleton = SingletonFactory.GetInstance(OType);
                        if (Singleton != null)
                            return Singleton;
                    }

                    UnityEngine.Debug.LogFormat("Registered {0} Stack {1}", OType, UnityEngine.StackTraceUtility.ExtractStackTrace());
                    UnityEngine.GameObject Container = IsSingleton ?
                        UnityEngine.GameObject.Find("Singleton Container") :
                        UnityEngine.GameObject.Find("Instance Container");

                    if (Container == null)
                    {
                        Container = IsSingleton ?
                            new UnityEngine.GameObject("Singleton Container") :
                            new UnityEngine.GameObject("Instance Container");
                        Container.AddComponent<Local.DontDestroy>();
                    }
                    return Container.GetComponent(OType) ?? Container.AddComponent(OType);
                }
                else if (typeof(ISingleton).IsAssignableFrom(OType) && CallFactory)
                    return SingletonFactory.GetInstance(OType);
                else
                    return Activator.CreateInstance(OType);
#else
            if (typeof(ISingleton).IsAssignableFrom(OType) && CallFactory)
                return SingletonFactory.GetInstance(OType);
            else
                return Activator.CreateInstance(OType);
#endif
            //}
        }
    }
}

