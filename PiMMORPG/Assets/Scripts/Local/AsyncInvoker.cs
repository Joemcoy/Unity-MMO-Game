using Scripts.Local.Helper;
using System;
using System.Collections;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Scripts.Local
{
    public class AsyncInvoker : MonoBehaviour
    {
        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        IEnumerator Do(Func<IEnumerator> Operation)
        {
            var Enumerator = Operation();
            while (Enumerator != null && Enumerator.MoveNext())
                yield return Enumerator.Current;
            yield return null;

            Destroy(gameObject);
        }


        public static void Create(Func<IEnumerator> Operation)
        {
            if (EditorHelper.IsPlaying)
            {
                var Object = new GameObject("AsyncInvoker");
                var Invoker = Object.AddComponent<AsyncInvoker>();
                Invoker.StartCoroutine(Invoker.Do(Operation));
            }
            else
            {
                var Enumerator = Operation();
                while (Enumerator != null && Enumerator.MoveNext())
                {
                    if (Enumerator.Current != null)
                        Create(() => Enumerator.Current as IEnumerator);
                }
            }
        }
    }
}
