using System;

namespace tFramework.Exceptions
{
	public class NotAssignableException<TInterface> : Exception
	{
		public NotAssignableException(Type target) : base(string.Format("The type {0} dont implements interface (or extends an abstract class) {1}", target.Name, typeof(TInterface).Name))
		{
		}

        public static void Test(Type target)
        {
            if (!typeof(TInterface).IsAssignableFrom(target))
                throw new NotAssignableException<TInterface>(target);
        }
	}
}