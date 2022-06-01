using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using tFramework.Factories;
using tFramework.Interfaces;
using UnityEngine;

namespace Scripts.Local
{
    [DisallowMultipleComponent]
    public abstract class SingletonBehaviour : MonoBehaviour, ISingleton
    {
        void Awake()
        {
            SingletonFactory.RegisterSingleton(GetType(), this);
        }

        public virtual void Created()
        {
            DontDestroyOnLoad(transform.root.gameObject);
        }
        public virtual void Destroyed() { }
    }

    public abstract class SingletonBehaviour<TSingleton> : SingletonBehaviour
        where TSingleton : SingletonBehaviour<TSingleton>
    {
        public static TSingleton Instance
        { get { return SingletonFactory.GetSingleton<TSingleton>(); } }
    }
}