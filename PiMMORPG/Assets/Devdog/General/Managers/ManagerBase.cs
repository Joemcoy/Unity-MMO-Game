using System;
using System.Collections.Generic;
using UnityEngine;

namespace Devdog.General
{
    [DisallowMultipleComponent]
    public abstract class ManagerBase<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                }

                return _instance;
            }
            set { _instance = value; }
        }

        protected virtual void Awake()
        {
            _instance = this as T;
        }

        protected virtual void Start()
        {
            
        }
    }
}
