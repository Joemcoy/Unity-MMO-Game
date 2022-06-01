using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Scripts.Local
{
    public class SafeInvoker : SingletonBehaviour<SafeInvoker>
    {
        int Counter;
        Queue<Action> Calls;

        public override void Created()
        {
            base.Created();
            Calls = new Queue<Action>();
        }

        public static void Create(Action Call)
        {
            Instance.Calls.Enqueue(Call);
        }

        void Update()
        {
            while (Calls != null && Calls.Count > 0)
            {
                StartCoroutine(Caller(Calls.Dequeue(), Interlocked.Increment(ref Counter)));
            }
        }

        IEnumerator Caller(Action Call, int Counter)
        {
            yield return null;
            Call();
            yield return null;
            Interlocked.Decrement(ref this.Counter);
        }
    }
}