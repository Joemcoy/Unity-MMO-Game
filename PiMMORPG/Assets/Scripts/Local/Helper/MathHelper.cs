using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Scripts.Local.Helper
{
    public static class MathHelper
    {
        public static float Lerp(ref float from, float to, float t)
        {
            return from = Mathf.Lerp(from, to, t);
        }
    }
}