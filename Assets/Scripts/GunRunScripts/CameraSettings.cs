using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.braineeeeDevs.gr
{
    [CreateAssetMenu()]
    public class CameraSettings : ScriptableObject
    {
        public float cameraAngle = 60f, focusLength = 10000f, innerFocus = 1f, outerFocus = 9999f;
        public RenderMode renderMode = RenderMode.WorldSpace;
    }
}