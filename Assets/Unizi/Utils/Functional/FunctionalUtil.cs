using System;
using UnityEngine;

namespace Unizi.Utils.Functional
{
    public static class FunctionalUtil
    {
        public static bool Approximately(double a, double b) 
            => Math.Abs(b - a) <= 0.005 * Math.Max(Math.Max(Math.Abs(a), Math.Abs(b)), 1e-6);

        public static bool Approximately(Vector3 a, Vector3 b)
            => Approximately(a.x, b.x) && Approximately(a.y, b.y) && Approximately(a.z, b.z);
    }
}