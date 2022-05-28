using UnityEngine;

namespace Unizi.Utils.Functional
{
    public class Rotation : IMovingAlgorithm
    {
        private readonly Quaternion to;
        private readonly float speed;
        private readonly GameObject rotateObject;

        public Rotation(GameObject g, Vector3 v, float f = 1)
        {
            rotateObject = g;
            to = Quaternion.Euler(v);
            speed = f;
        }

        public void Move() =>
            rotateObject.transform.rotation =
                Quaternion.Slerp(rotateObject.transform.rotation, to, Time.deltaTime * speed);

        public bool Check()
            => rotateObject == null || FunctionalUtil.Approximately(rotateObject.transform.rotation.eulerAngles, to.eulerAngles);
    }
}