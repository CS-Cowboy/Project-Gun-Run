using UnityEngine;


namespace com.braineeeeDevs.gr
{
    [RequireComponent(typeof(AnimationCurve))]
    public class ObjectAttributes : ScriptableObject
    {
        /// <summary>
        /// We are using metric in this simulation.
        /// </summary>
        public float topSpeed, mass, drag;
        public string poolID = System.Guid.Empty.ToString();
        public uint poolCapacity = 5;

    }
}