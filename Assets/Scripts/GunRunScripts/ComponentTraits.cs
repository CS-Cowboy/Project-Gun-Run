using UnityEngine;

namespace com.braineeeeDevs.gr
{
    [RequireComponent(typeof(AnimationClip))]

    [CreateAssetMenu()]
    public class ComponentTraits : ScriptableObject
    {
        public CameraSettings cameraTraits;
        public EngineTraits engineTraits;
        public TransmissionTraits trannyTraits;    //Suck on that, liberals.  xD
        public string poolID = System.Guid.Empty.ToString();
        public float maxHits = 5f, brakeTorque = 200f, antirollForce = 400f, topSpeed, mass = 1817f, drag= 0.2f, turnRadius = 18f;
        public float headlampRange, headlampSpotAngle, foglampRange = 60f, foglampAngle = 45f, headlampAxisAngle = 15f , foglampAxisAngle = 10f, suspensionDistance = 0.1f, forwardFrictionStiffness = 1.25f, sideFrictionStiffness = 1.15f, wheelBaseLength, wheelBaseWidth;
        public uint poolCapacity = 5;
        public AnimationCurve hitsCurve;
        public Canvas hudPrefab;
        public Mesh mesh;
    }
}