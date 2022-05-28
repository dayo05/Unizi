using System;
using System.Collections.Generic;
using UnityEngine;
using Unizi.Utils;
using Unizi.Utils.Functional;

namespace Unizi
{
    [AddComponentMenu("Unizi/Camera")]
    public class PresentationCamera : MovingManager
    {
        private Camera cam;
        public List<CamLocationData> CameraPos = new List<CamLocationData>{new (0, 0, 5)};
        public int currentPos = 0;
        public float zoomSpeed = 2;
        public float linearSpeed = 3;

        protected override void Initialize()
        {
            if (!TryGetComponent(out cam))
                throw new InvalidOperationException("You should register this component to camera.");
            if (CameraPos.Count == 0)
                throw new IndexOutOfRangeException("You should add 1+ locations for camera");

            cam.transform.position = CameraPos[currentPos].GetVector();
            cam.orthographicSize = CameraPos[currentPos].GetScale();
        
            UIUtil.Initialize();
        }

        protected override void RedirectUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                cam.transform.position = CameraPos[currentPos].GetVector();
                cam.orthographicSize = CameraPos[currentPos].GetScale();
            }
            base.RedirectUpdate();
        }

        private bool isNext = true;
        protected override void MoveNext()
        {
            if (IsMoving)
            {
                if (!isNext)
                    currentPos++;
                MovingObjects.Clear();
                gameObject.transform.position = CameraPos[currentPos].GetVector();
                cam.orthographicSize = CameraPos[currentPos].GetScale();
            }
            else if (currentPos != CameraPos.Count - 1)
            {
                isNext = true;
                MovingObjects.Add(new LinearMove(gameObject, CameraPos[++currentPos].GetVector(), linearSpeed));
                MovingObjects.Add(new LerpCamSize(cam, CameraPos[currentPos].GetScale(), zoomSpeed));
            }
        }

        protected override void MovePrevious()
        {
            if (IsMoving)
            {
                if (isNext)
                    currentPos--;
                MovingObjects.Clear();
                gameObject.transform.position = CameraPos[currentPos].GetVector();
                cam.orthographicSize = CameraPos[currentPos].GetScale();
            }
            else if (currentPos != 0) 
            {
                isNext = false;
                MovingObjects.Add(new LinearMove(gameObject, CameraPos[--currentPos].GetVector(), linearSpeed));
                MovingObjects.Add(new LerpCamSize(cam, CameraPos[currentPos].GetScale(), zoomSpeed));
            }
        }
    }
}
