using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;


    namespace com.braineeeeDevs.gunRun
    {
        [RequireComponent(typeof(AnimationCurve))]
        public class ObjectAttributes : ScriptableObject
        {
        public float topSpeed, brakingForce = 2000f, headlampRange = 20f, headlampAngle = 16f,foglampRange = 40f, foglampAngle = 20f, horsePower = 169.0f,finalDrive,suspensionDistance = 0.1f,  forwardFrictionStiffness = 1.25f, sideFrictionStiffness = 1.15f, wheelMassInKg = 20f, tachometerLimitInRPM = 8000.0f,massInKg, drag, fireDelayTime, reloadDelayTime, wheelBaseLength, wheelBaseWidth, turnRadius, antiRollForce;
        public bool hasAWD = false;
        public uint totalWheelCount = 4;
        public GameObject hudPrefab;
        public float desiredSlip = 0.2f, betaControlParam = 0.02f, kControlParam = 0.1f;
        public AnimationCurve engineTorqueToGearsCurve;
    }

}