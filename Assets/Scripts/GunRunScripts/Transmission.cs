using UnityEngine;

namespace com.braineeeeDevs.gr
{
    public class Transmission : VehicleComponent
    {
        public float outputTorque = 0f, gearRatio = 0f, driveDirection = +1f;
        public bool torqueConverterEngaged = true;

        /// <summary>       
        /// Operates the transmission.
        /// </summary>
        /// <param name="engineSpeed">Speed input to the transmission (rev/min).</param>
        public override void Operate(float engineSpeed)
        {
            var damageRatio = ComputeDamage();
            var torque = damageRatio * Mathf.Abs(MathUtilities.GetTorqueFrom(engineSpeed, owner.vehicleTraits.enginePower));
            if (torqueConverterEngaged)
            {
                gearRatio = owner.vehicleTraits.GetGearRatioFromTorque(torque) * owner.vehicleTraits.finalDrive;
                outputTorque = damageRatio * driveDirection * torque * gearRatio;
            }
            else
            {
                outputTorque = 0f;
            }
            // Debug.Log(string.Format("Input Engine Speed -> {0}, Input Torque -> {1}, Output Torque -> {2}, GearRatio-> {3}", engineSpeed, torque, outputTorque, gearRatio));
        }

        public void ToggleShift()
        {
            driveDirection = -1f * driveDirection;
        }

        protected float ComputeDamage()
        {
            switch (currentThermalCondition)
            {
                case DAMAGE_CONDITION.LEVEL_1:
                    return 1.0f;
                case DAMAGE_CONDITION.LEVEL_2:
                    return 0.90f;
                case DAMAGE_CONDITION.LEVEL_3:
                case DAMAGE_CONDITION.LEVEL_4:
                    return 0.75f;
                case DAMAGE_CONDITION.LEVEL_5:
                    return 0.40f;
                case DAMAGE_CONDITION.LEVEL_6:
                case DAMAGE_CONDITION.LEVEL_7:
                    return 0.10f;
            }
            return 1.0f;
        }
    }
}