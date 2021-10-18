
using UnityEngine;


namespace com.braineeeeDevs.gr
{
    [CreateAssetMenu()]
    public class TransmissionTraits : ScriptableObject
    {
        public float finalDrive = 1.54f;
        public AnimationCurve engineSpeedToTorqueGearCurveForCVT;
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
                return gearSet[0];
            }
            else
            {
                Debug.LogWarning("A vehicle's ObjectAttributes gearSet and gearTorque array lenghts are not equal, these must have equal sizes with definite values assigned per element. Defaulting to 1.0 gear ratio");
                return 1f;
            }
        }
    }
}