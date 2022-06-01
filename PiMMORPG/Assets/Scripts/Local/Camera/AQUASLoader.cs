using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Scripts.Local.Control;

public class AQUASLoader : MonoBehaviour
{
    public AQUAS_LensEffects lens;
    private void Start()
    {
        var water = FindObjectOfType<WaterElevator>();

        if (lens != null && water != null)
        {
            lens.gameObjects.waterPlanes[0] = water.gameObject;
            lens.gameObjects.mainCamera = gameObject;
            lens.enabled = true;
        }
    }
}