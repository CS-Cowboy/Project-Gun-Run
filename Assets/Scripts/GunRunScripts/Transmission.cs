using UnityEngine;

namespace com.braineeeeDevs.gr
{
    public class Transmission : VehicleComponent
    {
        public float outputTorque = 0f, gearRatio = 0f, driveDirection = +1f;
        public bool torqueConverterEngaged = true;
        ITakeDamage damage;
        public void Awake()
        {
            damage = this as ITakeDamage;
        }

        /// <summary>       
        /// Operates the transmission.
        /// </summary>
        /// <param name="engineSpeed">Speed input to the transmission (rev/min).</param>
        public override void Operate(float engineSpeed)
        {
            var effectiveness = damage.EvaluateHits(owner.vehicleTraits.components.transmissionHitsCurve);
            driveDirection = engineSpeed >= 0f ? +1f : -1f;
            var torque = Mathf.Abs(MathUtilities.GetTorqueFrom(engineSpeed, owner.vehicleTraits.enginePower));
            if (torqueConverterEngaged)
            {
                gearRatio = owner.vehicleTraits.GetGearRatioFromTorque(torque) * owner.vehicleTraits.finalDrive;
                outputTorque = effectiveness * driveDirection * torque * gearRatio;
            }
            else
            {
                outputTorque = 0f;
            }
            Debug.Log(string.Format("Input Engine Speed -> {0}, Input Torque -> {1}, Output Torque -> {2}, GearRatio-> {3}", engineSpeed, torque, outputTorque, gearRatio));
        }


    }
}