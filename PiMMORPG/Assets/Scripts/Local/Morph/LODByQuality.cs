using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using MORPH3D;

namespace Scripts.Local.Morph
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(M3DCharacterManager))]
    public class LODByQuality : MonoBehaviour
    {
        M3DCharacterManager Manager;
        //public int Last = -1;
        //public float LOD = 0;

        void Awake()
        {
            Manager = GetComponent<M3DCharacterManager>();
            InvokeRepeating("UpdateLOD", 10f, 3f);
        }

        void UpdateLOD()
        {
            //if (Last != QualitySettings.GetQualityLevel())
            if (Manager.currentLODLevel != QualitySettings.lodBias)
            {
                Manager.SetLODLevel(QualitySettings.lodBias);
                Manager.SyncAllBlendShapes();
                /*Last = QualitySettings.GetQualityLevel();
                LOD = Last == 0 ? 0 : Last / (float)(QualitySettings.names.Length - 1);

                Manager.SetLODLevel(LOD);
                Manager.SyncAllBlendShapes();*/
            }
        }
    }
}