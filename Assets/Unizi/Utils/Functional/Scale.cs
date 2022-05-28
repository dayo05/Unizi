using System;
using UnityEngine;

namespace Unizi.Utils.Functional
{
    public class Scale : IMovingAlgorithm
    {
        private readonly Vector3 to;
        private readonly float speed;
        private readonly GameObject scaleObject;

        public Scale(GameObject g, Vector3 v, float f = 0.33f)
        {
            if (f >= 100) throw new Exception("speed cannot be bigger then 100");
            to = v;
            speed = f;
            scaleObject = g;
        }

        public void Move()
            => scaleObject.transform.localScale = Vector3.Lerp(scaleObject.transform.position, to, speed * Time.deltaTime);
        
        public bool Check()
            => scaleObject == null || FunctionalUtil.Approximately(to, scaleObject.transform.localScale);
    }
}