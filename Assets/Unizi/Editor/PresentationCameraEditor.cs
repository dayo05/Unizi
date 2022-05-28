using System;
using UnityEditor;
using UnityEngine;

namespace Unizi.Editor
{
    [CustomEditor(typeof(PresentationCamera))]
    public class PresentationCameraEditor: UnityEditor.Editor
    {
        private PresentationCamera cam => (PresentationCamera) target;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (cam.currentPos < 0)
                cam.currentPos = 0;
            
            if (cam.CameraPos.Count == 0) return;
            if (cam.currentPos >= cam.CameraPos.Count)
                cam.currentPos = cam.CameraPos.Count - 1;

            if (Application.isPlaying) return;
            cam.transform.position = cam.CameraPos[cam.currentPos].GetVector();
            cam.GetComponent<Camera>().orthographicSize = cam.CameraPos[cam.currentPos].GetScale();
        }

        public void OnValidate()
        {
            if (cam.currentPos < 0)
                cam.currentPos = 0;
            
            if (cam.CameraPos.Count == 0) return;
            if (cam.currentPos >= cam.CameraPos.Count)
                cam.currentPos = cam.CameraPos.Count - 1;

            cam.transform.position = cam.CameraPos[cam.currentPos].GetVector();
            cam.GetComponent<Camera>().orthographicSize = cam.CameraPos[cam.currentPos].GetScale();
        }
    }
}