using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Scripts.Local.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class FadeAudio : MonoBehaviour
    {
        public enum FadeMode
        {
            FadeIn = 2,
            FadeOut = 4,
            FadeInOut = FadeIn | FadeOut
        }

        public FadeMode Mode = FadeMode.FadeIn;
        public int FadePercentage = 7;
        public float MaximumVolume = 0;

        AudioSource Source;

        void Start()
        {
            Source = GetComponent<AudioSource>();
            if (MaximumVolume == 0)
                MaximumVolume = Source.volume;
        }
        
        void Update()
        {
            if (Source.clip)
            {
                var Percentage = Mathf.RoundToInt((Source.time * 100) / Source.clip.length);
                var OutP = 100 - FadePercentage;

                if ((Mode & FadeMode.FadeIn) == FadeMode.FadeIn && Percentage <= FadePercentage)
                    Source.volume = Percentage > FadePercentage ? MaximumVolume : Percentage / (float)FadePercentage;
                else if ((Mode & FadeMode.FadeOut) == FadeMode.FadeOut && Percentage >= OutP)
                    Source.volume = Percentage < OutP ? MaximumVolume : MaximumVolume - ((100 - Percentage) / (float)OutP);
                else if (Percentage > FadePercentage && Percentage < OutP)
                    Source.volume = MaximumVolume;
            }
        }
    }
}
