
using UnityEngine;
namespace com.braineeeeDevs.gr
{
    public class SteeringMechanism : VehicleComponent
    {
        [SerializeField] protected Wheel left, right;
        protected float[] ComputeAckermann(float x)
        {   
            float inner, outer;
            inner = Mathf.Atan(owner.target.Traits.wheelBaseLength * x / (owner.target.Traits.turnRadius - (owner.target.Traits.wheelBaseWidth * 0.5f))) * Mathf.Rad2Deg;
            outer = Mathf.Atan(owner.target.Traits.wheelBaseLength * x / (owner.target.Traits.turnRadius + (owner.target.Traits.wheelBaseWidth * 0.5f))) * Mathf.Rad2Deg;
            return new float[2] { x > 0f ? inner : outer, x > 0f ? outer : inner };
        }
        public Wheel[] Wheels
        {
            set
            {
                left = value[0];
                right = value[1];
            }
        }
        public override void Start()
        {
            Traits = owner.target.Traits;
        }
        public override void Operate(float input)
        {
            base.Operate(input);
            var angles = ComputeAckermann(input);
            if(left.Collider != null)
                left.Collider.steerAngle = angles[0] * effectiveness;
            if(right.Collider != null)
                right.Collider.steerAngle = angles[1] * effectiveness;
        }
    }
}
