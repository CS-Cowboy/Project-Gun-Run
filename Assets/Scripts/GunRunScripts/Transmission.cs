using UnityEngine;

namespace com.braineeeeDevs.gunRun
{
    public class Transmission : VehicleComponent
    {
        public float outputTorque = 0f, gearRatio = 0f; 
        public bool torqueConverterEngaged = true;
        public override void Operate(float engineSpeedInRPM)
        {
            var torque = 0f;
            if (torqueConverterEngaged)
            {
                torque = Mathf.Abs(MathUtilities.hpToNM * owner.traits.horsePower / (engineSpeedInRPM * owner.wheels[0].tireDiameterInMeters));
                gearRatio = owner.traits.engineTorqueToGearsCurve.Evaluate(torque) * owner.traits.finalDrive;
                outputTorque = torque * gearRatio * owner.engine.driveDirection;
            }
            else
            {
                outputTorque = 0f;
            }
            Debug.Log(string.Format("Input Torque -> {0}, Output Torque -> {1}, GearRatio-> {2}", torque, outputTorque, gearRatio));
        }
    }
}