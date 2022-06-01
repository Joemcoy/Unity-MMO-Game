using System;
using System.Collections;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Scripts.Local.Control
{
    public class WaterElevator : MonoBehaviour
    {
        public float normalLevel, currentLevel, unitPerSecond = 1f;
        public float euler = 0.25f;

        public Vector3 pos;
        private void Start()
        {
            currentLevel = 0f;
            pos = transform.position;
            time = Time.time;
        }

        float time;
        void FixedUpdate()
        {
            var level = normalLevel + currentLevel;
            if (transform.position.y != level)
            {
                pos.y = Mathf.Lerp(pos.y, pos.y + (pos.y > level ? -1 : 1) * unitPerSecond * (Time.time - time), euler);
                if (pos.y > level && pos.y - level < 0.025 || pos.y < level && level - pos.y < 0.025)
                    pos.y = level;
            }
            time = Time.time;
            transform.position = pos;
        }
    }
}