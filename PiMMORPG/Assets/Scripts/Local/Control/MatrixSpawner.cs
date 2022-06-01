using System;
using System.Collections;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Scripts.Local.Control
{
    public class MatrixSpawner : MonoBehaviour
    {
        public GameObject[] prefabs;
        public Terrain @base;
        public int width = 1, height = 1;
        public float interval = 0, offsetX = 0, offsetZ = 0;
        public bool spawning = false, pool = false, pooled = false;

        Coroutine[] spawnCoroutines;
        GameObject container, prefabPool;
        int sh, sw;

        public float mh = 25;

        private void OnGUI()
        {
            GUI.enabled = prefabs != null && prefabs.Length > 0 && !spawning;
            GUILayout.BeginArea(new Rect(10, 10, 360, 200), GUI.skin.box);
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Matrix Width");
            width = Mathf.RoundToInt(GUILayout.HorizontalSlider(width, 1, 100, GUILayout.ExpandWidth(true), GUILayout.Height(mh)));
            var wf = GUILayout.TextField(width.ToString(), GUILayout.ExpandWidth(true), GUILayout.Height(mh));
            width = Convert.ToInt32(string.IsNullOrEmpty(wf) ? "0" : wf);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Matrix Height");
            height = Mathf.RoundToInt(GUILayout.HorizontalSlider(height, 1, 100, GUILayout.ExpandWidth(true), GUILayout.Height(mh)));
            var hf = GUILayout.TextField(height.ToString(), GUILayout.ExpandWidth(true), GUILayout.Height(mh));
            height = Convert.ToInt32(string.IsNullOrEmpty(hf) ? "0" : hf);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("X Offset");
            offsetX = GUILayout.HorizontalSlider(offsetX, 0, 10, GUILayout.ExpandWidth(true), GUILayout.Height(mh));
            GUILayout.Label(offsetX.ToString());
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Z Offset");
            offsetZ = GUILayout.HorizontalSlider(offsetZ, 0, 10, GUILayout.ExpandWidth(true), GUILayout.Height(mh));
            GUILayout.Label(offsetZ.ToString());
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Linear interval");
            interval = GUILayout.HorizontalSlider(interval, 0, 2, GUILayout.ExpandWidth(true), GUILayout.Height(mh));
            GUILayout.Label(interval.ToString());
            GUILayout.EndHorizontal();

            var pool = GUILayout.Toggle(this.pool, "Pooled spawner", GUILayout.ExpandWidth(true));
            /*if (pool != this.pool)
            {
                StartCoroutine(ClearMatrix());
                pooled = false;
            }*/
            this.pool = pool;

            GUILayout.Label(string.Format("Total of {0} instances to be spawned", width * height));
            GUILayout.Label(string.Format("Total of {0} instances as been spawned", transform.childCount));
            GUILayout.EndVertical();
            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(10, Screen.height - mh * 2 - 10, Screen.width - 20, mh * 2));
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            GUI.enabled = !pool || pooled;

            if (pool)
            {
                if (GUILayout.Button("Spawn pool", GUILayout.ExpandWidth(true), GUILayout.Height(mh)))
                    StartCoroutine(StartSpawn(prefabPool));
            }
            else
            {
                foreach (var prefab in prefabs)
                {
                    if (GUILayout.Button("Spawn " + prefab.name, GUILayout.ExpandWidth(true), GUILayout.Height(mh)))
                    {
                        StartCoroutine(StartSpawn(prefab));
                    }
                }
            }

            if (GUILayout.Button("Clear matrix", GUILayout.ExpandWidth(true), GUILayout.Height(mh)))
            {
                StartCoroutine(ClearMatrix());
            }
            GUI.enabled = spawning;

            if (GUILayout.Button("Stop spawn", GUILayout.ExpandWidth(true), GUILayout.Height(mh)))
            {
                foreach (var spawnCoroutine in spawnCoroutines)
                    StopCoroutine(spawnCoroutine);
                spawning = false;
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            GUI.enabled = pool;
            foreach (var prefab in prefabs)
            {
                if (GUILayout.Button("Pool for " + prefab.name, GUILayout.ExpandWidth(true), GUILayout.Height(mh)))
                {
                    prefabPool = prefab;

                    if (container)
                        Destroy(container);
                    container = new GameObject("Container");
                    for (int i = 0; i < width * height; i++)
                    {
                        var instance = Instantiate(prefab);
                        instance.transform.SetParent(container.transform);
                        pooled = true;
                    }
                }
            }

            GUI.enabled = pool && pooled;
            if (GUILayout.Button("Clear pool", GUILayout.ExpandWidth(true), GUILayout.Height(mh)))
            {
                Destroy(container);
                pooled = false;
            }
            GUI.enabled = true;
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndArea();
            GUI.enabled = true;
        }

        IEnumerator ClearMatrix()
        {
            spawning = true;
            if (container)
                if (pool)
                {
                    for (int y = 0; y < sh; y++)
                        StartCoroutine(DisableLine(y));
                }
                else
                    Destroy(container);
            yield return null;
            spawning = false;
        }

        IEnumerator DisableLine(int y)
        {
            int x = 0;
            while (container && x < sw)
            {
                container.transform.GetChild(x++ * sh + y).gameObject.SetActive(false);
                yield return null;
            }
            yield return null;
        }

        IEnumerator StartSpawn(GameObject prefab)
        {
            if (spawnCoroutines != null)
                foreach (var spawnCoroutine in spawnCoroutines)
                    StopCoroutine(spawnCoroutine);
            yield return StartCoroutine(ClearMatrix());

            sw = width;
            sh = height;

            if (!pool)
                container = new GameObject("Container");
            spawnCoroutines = new Coroutine[height];
            for (int y = 0; y < height; y++)
            {
                var nZ = (y + 1) * (prefab.transform.localScale.z + prefab.transform.localScale.z / 2);
                spawnCoroutines[y] = StartCoroutine(SpawnMatrix(prefab, container, y, nZ));
            }
        }

        IEnumerator SpawnMatrix(GameObject prefab, GameObject container, int z, float nZ)
        {
            spawning = true;
            float nX = 0;
            int x = 0;
            /*for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {*/
            while (x < width)// * height)
            {
                if (nX == 0)
                    nX = prefab.transform.localScale.x;

                var instance = pool ? container.transform.GetChild(x * height + z).gameObject : Instantiate(prefab);
                instance.transform.SetParent(container.transform);
                var pos = new Vector3(nX + offsetX * x, 0, nZ + offsetZ * z);
                pos.y = (@base == null ? 0 : @base.SampleHeight(pos)) + prefab.transform.localScale.y / 1.5f;
                instance.transform.position = pos;
                
                if (!instance.activeInHierarchy)
                    instance.SetActive(true);

                if ((x + 1) % (width == 0 ? 1 : width) != 0)
                {
                    nX += prefab.transform.localScale.x + prefab.transform.localScale.x / 2;
                }
                else
                {
                    nX = 0;
                }
                x++;
                yield return new WaitForSeconds(interval);
                yield return null;
            }
            //}
            spawning = false;
        }
    }
}