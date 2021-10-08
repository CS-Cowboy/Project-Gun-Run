using UnityEngine;


namespace com.braineeeeDevs.gr
{
    [RequireComponent(typeof(AnimationCurve))]
    public class ObjectAttributes : ScriptableObject
    {
        /// <summary>
        /// We are using metric in this simulation.
        /// </summary>
        public float topSpeed, brakingForce = 200f, headlampRange = 20f, headlampAngle = 16f, highBeamRange = 40f, highBeamSpotAngle = 40f, mass, drag, fireDelayTime, reloadDelayTime;
        public uint poolCapacity = 5;
        public GameObject hudPrefab;

    }
}