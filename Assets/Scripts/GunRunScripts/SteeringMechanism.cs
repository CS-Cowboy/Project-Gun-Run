
using UnityEngine;
namespace com.braineeeeDevs.gr
{
    public class SteeringMechanism : VehicleComponent
    {
        protected Wheel left, right;
        protected float[] ComputeAckermann(float x)
        {
            float inner, outer;
            inner = Mathf.Atan(owner.target.Traits.wheelBaseLength / (owner.target.Traits.turnRadius - owner.target.Traits.wheelBaseWidth * 0.5f)) * Mathf.Rad2Deg;
            outer = Mathf.Atan(owner.target.Traits.wheelBaseLength / (owner.target.Traits.turnRadius + owner.target.Traits.wheelBaseWidth * 0.5f)) * Mathf.Rad2Deg;
            return new float[2] { x > 0f ? inner : outer, x > 0f ?  outer : inner };
        }
        public Wheel Left
        {
            set
            {
                left = value;
            }
        }
        public Wheel Right
        {
            set
            {
                right = value;
            }
        }
        public override void Operate(float input)
        {
            base.Operate(input);
            var angles = ComputeAckermann(input);
            left.Collider.steerAngle = angles[0] * effectiveness;
            right.Collider.steerAngle = angles[1] * effectiveness;
        }
    }
}
