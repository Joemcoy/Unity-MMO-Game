using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Scripts.Local.Climate
{
    public class UnistormHelper : MonoBehaviour
    {
        public ParticleSystem RainParticle, RainSplash, RainMist;
        public ParticleSystem SnowParticle, SnowDust;
        public ParticleSystem LightingBugs, WindyLeaves;

        public void CopyTo(UniStormWeatherSystem_C unistorm, GameObject camera)
        {
            unistorm.rain = RainParticle;
            unistorm.rainSplashes = RainSplash;
            unistorm.rainMist = RainMist;
            unistorm.snow = SnowParticle;
            unistorm.snowMistFog = SnowDust;
            unistorm.butterflies = LightingBugs;
            unistorm.windyLeaves = WindyLeaves;
            unistorm.PlayerObject = transform.root.gameObject;
            unistorm.cameraObject = camera;
            unistorm.sunTrans = GameObject.FindGameObjectWithTag("Sun");
            unistorm.enabled = true;
        }

        void Reset()
        {
            Destroy(gameObject);
        }
    }
}