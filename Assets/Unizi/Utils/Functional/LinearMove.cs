using UnityEngine;

namespace Unizi.Utils.Functional
{
    public class LinearMove : IMovingAlgorithm
    {
        private readonly Vector3 to;
        private readonly float speed;
        private readonly GameObject moveObject;

        public LinearMove(GameObject g, Vector3 v, float f = 3)
        {
            to = v;
            speed = f;
            moveObject = g;
        }

        public void Move() =>
            moveObject.transform.position = Vector3.Lerp(moveObject.transform.position, to, speed * Time.deltaTime);

        public bool Check()
            => moveObject is null || FunctionalUtil.Approximately(to, moveObject.transform.position);
    }
}