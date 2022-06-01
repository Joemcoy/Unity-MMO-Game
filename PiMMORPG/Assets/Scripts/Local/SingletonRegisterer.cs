using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using tFramework.Factories;
using tFramework.Interfaces;

namespace Scripts.Local
{
    public class SingletonRegisterer : MonoBehaviour
    {
        public bool Permanent = false;
        public List<ISingleton> Singletons;

        void Start()
        {
            Singletons = new List<ISingleton>();
            if (Permanent)
                DontDestroyOnLoad(gameObject);

            foreach (var Singleton in GetComponentsInChildren<ISingleton>(true))
            {
                Singletons.Add(SingletonFactory.RegisterSingleton(Singleton.GetType(), Singleton));
            }
            Application.Register(this);
        }
    }
}
