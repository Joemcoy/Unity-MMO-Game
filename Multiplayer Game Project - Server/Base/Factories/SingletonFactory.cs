using System;
using System.Linq;
using System.Collections.Generic;

using Base.Data.Abstracts;
using Base.Data.Interfaces;
using Base.Data.DispatcherBases;
using Base.Data.EventArgs;
using Base.Data.Exceptions;
using Base.Helpers;

namespace Base.Factories
{
    public class SingletonFactory
    {
        static Dictionary<int, ISingleton> Singletons = new Dictionary<int, ISingleton>();
        static object syncLock = new object();

        public static event EventHandler<SingletonEventArgs> Registered, Destroyed;
        public static ISingleton[] Instances { get { return Singletons.Values.ToArray(); } }
        public static bool Initalized { get; private set; }

        public static ISingleton GetInstance(Type SingletonType)
        {
            if (typeof(ISingleton).IsAssignableFrom(SingletonType))
            {
                lock (syncLock)
                {
                    if (!Initalized)
                        Initalized = true;

                    if (!Singletons.ContainsKey(SingletonType.GetHashCode()))
                    {
                        if (!SingletonType.IsInterface && !SingletonType.IsAbstract)
                        {
                            ISingleton Singleton = (ISingleton)InstanceHelper.GetInstance(SingletonType, false);
                            
                            Singletons.Add(SingletonType.GetHashCode(), Singleton);
                            if (Registered != null)
                                Registered(null, new SingletonEventArgs(SingletonType, Singleton));

                            Singleton.Create();
                        }
                        else
                            return null;
                    }
                    return Singletons[SingletonType.GetHashCode()];
                }
            }
            else
                throw new NotImplementedInterfaceException(SingletonType, typeof(ISingleton));
        }

        public static TSingleton GetInstance<TSingleton>()
            where TSingleton : ISingleton
        {
            return (TSingleton)GetInstance(typeof(TSingleton));
        }

        public static void RegisterSingleton<TSingleton>(TSingleton Singleton)
            where TSingleton : ISingleton
        {
            RegisterSingleton(typeof(TSingleton), Singleton);
        }

        public static void RegisterSingleton(Type SingletonType, ISingleton Singleton)
        {
            lock (syncLock)
            {
                if (!Initalized)
                    Initalized = true;

                ISingleton Temp;
                if(!Singletons.TryGetValue(SingletonType.GetHashCode(), out Temp))
                {
                    if (Registered != null)
                        Registered(null, new SingletonEventArgs(SingletonType, Singleton));

                    Singletons.Add(SingletonType.GetHashCode(), Singleton);
                    Singleton.Create();
                }
            }
        }

        public static void Destroy<TSingleton>()
            where TSingleton : ISingleton
        {
            Destroy(typeof(TSingleton));
        }

        public static void Destroy(Type SingletonType)
        {
            if (typeof(ISingleton).IsAssignableFrom(SingletonType))
            {
                ISingleton Singleton = null;
                if (Singletons.TryGetValue(SingletonType.GetHashCode(), out Singleton))
                {
                    if (Destroyed != null)
                        Destroyed(null, new SingletonEventArgs(SingletonType, Singleton));

                    Singleton.Destroy();
                    Singletons.Remove(SingletonType.GetHashCode());
                }
            }
            else
                throw new NotImplementedInterfaceException(SingletonType, typeof(ISingleton));
        }

        public static void DestroyAll()
        {
            //lock (syncLock)
            //{
                ISingleton[] Temp = new ISingleton[Singletons.Count];
                Singletons.Values.CopyTo(Temp, 0);

                foreach (var Singleton in Temp)
                {
                    try
                    {
                        if (Destroyed != null)
                            Destroyed(null, new SingletonEventArgs(Singleton.GetType(), Singleton));

                        Singleton.Destroy();
                    }
                    catch (Exception)
                    {

                    }
                }
                Singletons.Clear();
            //}
        }
    }
}