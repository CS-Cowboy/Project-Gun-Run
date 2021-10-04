using UnityEngine;

namespace com.braineeeeDevs.gunRun
{
    public class Transmission : VehicleComponent
    {
        public float speed = 0f, outputTorque = 0f, gearRatio = 0f;
        public bool torqueConverterEngaged = true;
        public override void Operate(float engineSpeed)
        {
            if (torqueConverterEngaged)
            {
                float sign = engineSpeed < 0f ? -1f : +1f; 
                var absEngineSpd = Mathf.Abs(engineSpeed);
                 gearRatio = owner.traits.engineSpeedToGearsCurve.Evaluate(absEngineSpd);
                outputTorque = absEngineSpd * gearRatio * sign;
                Debug.Log("GearRatio-> " + gearRatio);
                Debug.Log("Output Tranny Torque-> " + outputTorque);
            }
        }
    }
}