using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Scripts.Local.Locomotion
{
    [ExecuteInEditMode]
    public class TerrainLimiter : MonoBehaviour
    {
        public Terrain terrain;
        public Vector3 limits = (Vector3.right + Vector3.forward) * 5f;

        private void Update()
        {
            if(terrain == null)
            {
                var Object = GameObject.FindGameObjectWithTag("MainTerrain");
                if (Object != null)
                    terrain = Object.GetComponent<Terrain>();
            }
            else
            {
                var position = transform.position;
                position.x = Mathf.Clamp(position.x, terrain.transform.position.x + limits.x, terrain.transform.position.x + terrain.terrainData.size.x - limits.z);
                position.z = Mathf.Clamp(position.z, terrain.transform.position.z + limits.z, terrain.transform.position.z + terrain.terrainData.size.z - limits.z);

                //var y = terrain.terrainData.GetHeight(Convert.ToInt32(position.x / terrain.terrainData.heightmapScale.x), Convert.ToInt32(position.z / terrain.terrainData.heightmapScale.z));
                var y = terrain.SampleHeight(position) + limits.y;
                if (position.y < y)
                    position.y = y;

                transform.position = position;
            }
        }
    }
}