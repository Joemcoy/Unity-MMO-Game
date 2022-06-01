using System;
using System.Collections;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Local
{
    public class Preload : MonoBehaviour
    {
        public Application app;

        void Start()
        {
            StartCoroutine(Load());
        }

        IEnumerator Load()
        {
            yield return new WaitForSeconds(2.5f);
            //var operation = SceneManager.LoadSceneAsync(2);
            //while (!operation.isDone)
            //  yield return null;
            app.gameObject.SetActive(true);
        }
    }
}