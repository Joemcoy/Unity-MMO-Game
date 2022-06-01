using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Base.Data.Abstracts
{
    public abstract class ACommand
    {
        private volatile object syncLock = new object();
        private Dictionary<string, object> Parameters = new Dictionary<string, object>();
        public abstract string Name { get; }

        public void SetParameter(string Name, object Value)
        {
            lock (syncLock)
            {
                Parameters[Name] = Value;
            }
        }

        public object GetParameter(string Name)
        {
            lock (syncLock)
            {
                return Parameters.ContainsKey(Name) ? Parameters[Name] : null;
            }
        }

        public TValue GetParameter<TValue>(string Name) 
        {
            object Value = GetParameter(Name);
            return Value == null ? default(TValue) : (TValue)Value;
        }

        public void ClearParameters()
        {
            lock(syncLock)
            {
                Parameters.Clear();
            }
        }

        public abstract bool Execute(params string[] Arguments);
    }
}
