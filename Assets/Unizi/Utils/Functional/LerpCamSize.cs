using System;
using UnityEngine;

namespace Unizi.Utils.Functional
{
    public class LerpCamSize: IMovingAlgorithm
    {
        private readonly float to;
        private readonly float speed;
        private readonly Camera moveObject;
        
        public LerpCamSize(Camera g, float v, float f = 2)
        {
            to = v;
            speed = f;
            moveObject = g;
        }

        public void Move() =>
            moveObject.orthographicSize = Mathf.Lerp(moveObject.orthographicSize, to, speed * Time.deltaTime);

        public bool Check()
            => moveObject is null || FunctionalUtil.Approximately(to, moveObject.orthographicSize);
    }
}