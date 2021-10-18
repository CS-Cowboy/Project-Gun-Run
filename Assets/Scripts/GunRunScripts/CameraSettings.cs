using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.braineeeeDevs.gr
{
    [CreateAssetMenu()]
    public class CameraSettings : ScriptableObject
    {
        public Vector3 localPositionAtStart;
        public float cameraAngle = 60f, focusLength = 10000f, innerFocus = 1f, outerFocus = 9999f;
        public float rollSpeed = 3f,  speedMultipler = 4f;
        public float  lookSpeed = 2f, lookAcceleration = 2f, camSpeed = 2f;
        public float zoomSpeed = 0.5f, innerZoomLimit = 5f, outerZoomLimit = 20f, lowerCamSpeedLimit = 2f, upperCamSpeedLimit = 100f, upperVerticalThetaLimit = 80f;
        public RenderMode renderMode = RenderMode.WorldSpace;
    }
}