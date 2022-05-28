using System;
using UnityEngine;

namespace Unizi.Utils
{
    [Serializable]
    public class CamLocationData
    {
        public Vector2 pos = new();
        public double scale = 5.0;

        public CamLocationData() { }
        public CamLocationData(double x, double y, double scale)
        {
            this.pos.x = (float)x;
            this.pos.y = (float)y;
            this.scale = scale;
        }

        public Vector3 GetVector()
            => new (pos.x, pos.y, -5);

        public float GetScale()
            => (float)scale;
    }
}