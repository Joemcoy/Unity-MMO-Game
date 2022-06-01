using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tFramework.Bases
{
    using Factories;
    using Interfaces;

    public abstract class BaseSingleton<T> : ISingleton
        where T : BaseSingleton<T>
    {
        public static T Instance { get { return SingletonFactory.GetSingleton<T>(); } }

        void ISingleton.Created() { this.Created(); }
        protected virtual void Created() { }

        void ISingleton.Destroyed() { this.Destroyed(); }
        protected virtual void Destroyed() { }
    }
}