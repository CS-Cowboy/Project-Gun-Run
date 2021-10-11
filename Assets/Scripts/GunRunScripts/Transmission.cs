using UnityEngine;

namespace com.braineeeeDevs.gr
{
    public class Transmission : VehicleComponent
    {
        public float outputTorque = 0f, gearRatio = 0f, driveDirection = +1f;
        public bool torqueConverterEngaged = true;
        public Differential frontDiff, rearDiff;
        public TransmissionTraits trans;
        ///FOR FUTURE REFERENCE:  TO INHERIT A METHOD: 1. Define virtual function in parent.
        /// 2. Define override in child.
        /// 3. Call Base.Method() from exactly ONE child.       Unity is stupid, SMH.
        public override void Start()
        {
            base.Start();
            var diffs = owner.target.GetComponentsInChildren<Differential>();
            frontDiff = diffs[0];
            rearDiff = diffs[1];
        }

        /// Operates the transmission.
        /// </summary>
        /// <param name="engineSpeed">Speed input to the transmission (rev/min).</param>

        //@CS-Cowboy    This works great. Don't MESS with it! 
        public override void Operate(float engineSpeed)
        {
            base.Operate();
            var torque = Mathf.Abs(MathUtilities.GetTorqueFrom(engineSpeed, owner.engine.engTraits.power));
            if (torqueConverterEngaged)
            {
                gearRatio = trans.GetGearRatioFromTorque(torque) * trans.finalDrive;
                outputTorque = effectiveness * driveDirection * torque * gearRatio;
            }                                           
            else
            {
                outputTorque = 0f;
            }
            frontDiff.Operate(outputTorque);
            rearDiff.Operate(outputTorque);
            Debug.Log(string.Format("Input Engine Speed -> {0}, Input Torque -> {1}, Output Torque -> {2}, GearRatio-> {3}", engineSpeed, torque, outputTorque, gearRatio));
        }
        //@CS-Cowboy    This works great. Don't MESS with it! 
        public void SetDriveDirection(float direction)
        {
            driveDirection = (direction == 0f ? 0f : 1f )* (direction > 0f ? +1f : -1f);
        }
        public void ShiftParkState()
        {
            torqueConverterEngaged = !torqueConverterEngaged;
        }
    }
}