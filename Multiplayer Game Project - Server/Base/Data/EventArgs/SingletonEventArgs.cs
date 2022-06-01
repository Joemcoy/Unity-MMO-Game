using System;
using Base.Data.Interfaces;

namespace Base.Data.EventArgs
{
	using EventArgs = System.EventArgs;
	public class SingletonEventArgs : EventArgs
    {
		public ISingleton Singleton { get; private set; }
        public Type SingletonType { get; private set; }

        public SingletonEventArgs(Type SingletonType, ISingleton Singleton)
        {
            this.SingletonType = SingletonType;
            this.Singleton = Singleton;
        }
    }
}