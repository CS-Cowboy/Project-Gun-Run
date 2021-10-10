using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.braineeeeDevs.gr
{
    [CreateAssetMenu()]
    public class VehicleTraits : ScriptableObject
    {

        public float brakingForce, highBeamRange, highbeamSpotAngle, headlampRange, headlampSpotAngle,foglampRange = 60f, foglampAngle = 45f, enginePower = 169.0f, finalDrive, suspensionDistance = 0.1f, forwardFrictionStiffness = 1.25f, sideFrictionStiffness = 1.15f, wheelMass = 20f, tachometerLimit = 8000.0f, wheelBaseLength, wheelBaseWidth, antiRollForce, turnRadius, idleSpeed = 20f;
        public GameObject hudPrefab;
        public ComponentTraits components;
        /// <summary>
        /// In case of CVT
        /// </summary>
        public AnimationCurve engineSpeedToTorqueGearCurve;
        /// <summary>
        /// Used in the gearSet variable in conjunction with GetGearRatioFromTorque() function to compute the current gear ratio.
        /// </summary>
        public float[] gearTorques;

        ///Actual gearset.
        public float[] gearSet;

        public float GetGearRatioFromTorque(float input)
        {
            if (gearSet.Length == gearTorques.Length)
            {
                for (int c = 0; c < gearSet.Length - 1; c++)
                {
                    if (input < gearTorques[c])
                    {
                        return gearSet[c];
                    }
                }
            }
            Debug.LogWarning("A vehicle's ObjectAttributes gearSet and gearTorque array lenghts are not equal, these must have equal sizes with definite values assigned per element. Defaulting to 1.0 gear ratio");
            return 1f;
        }
    }
}