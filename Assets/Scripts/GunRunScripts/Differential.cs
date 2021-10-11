
namespace com.braineeeeDevs.gr
{
    public class Differential : VehicleComponent
    {
        public Wheel left, right;

        /// <summary>
        /// Operates the Differential. You must call this every FixedUpdate regardless of its condition.
        /// </summary>
        /// <param name="inputTorque">The torque (Nm).</param>
        public override void Operate(float inputTorque)
        {
            base.Operate();
            left.Operate(effectiveness * inputTorque * 0.5f);
            right.Operate(effectiveness * inputTorque * 0.5f);
        }
    }
}