using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Base.Factories;
using Base.Data.Interfaces;
using Base.Helpers;

namespace Base.Data.Abstracts
{
    public class ASingleton<TSingleton> : ISingleton
        where TSingleton : ISingleton
    {
        private static TSingleton instance = default(TSingleton);
        private static object syncLock = new object();
        private static bool Initalized = false;

        public static TSingleton Instance
        {
            get
            {
                if (!Initalized)
                {
                    SingletonFactory.Destroyed += SingletonFactory_Destroyed;
                    Initalized = true;
                }

                lock (syncLock)
                {
                    if (instance == null)
                    {
                        instance = InstanceHelper.GetInstance<TSingleton>();
                        SingletonFactory.RegisterSingleton(instance);
                    }
                    return instance;
                }
            }
        }

        private static void SingletonFactory_Destroyed(object sender, EventArgs.SingletonEventArgs e)
        {
            if (e.SingletonType == typeof(TSingleton))
                instance = default(TSingleton);
        }

        void ISingleton.Create() { Created(); }
        void ISingleton.Destroy() { Destroyed(); }

        protected virtual void Created() { }
        protected virtual void Destroyed() { }
    }
}