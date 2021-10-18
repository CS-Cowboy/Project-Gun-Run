
using UnityEngine;

namespace com.braineeeeDevs.gr
{
    public class SwayBar : VehicleComponent
    {
        protected Wheel left, right;
        public Wheel[] Wheels
        {
            set
            {
                left = value[0];
                right = value[1];
            }
        }
        /// <summary>
        /// Computes the swaybar force to prevent vehicle roll.
        /// </summary>
        /// <returns>The anti-roll force.</returns>
        public override void Operate()
        {
            base.Operate();
            var lean = -1f * effectiveness * (owner.steering < 0f ? (ComputeForce(right) - ComputeForce(left)) : (ComputeForce(left) - ComputeForce(right)));
            owner.target.ApplyExtraForceAt(left.transform.localPosition, Vector3.up * lean);
            owner.target.ApplyExtraForceAt(right.transform.localPosition, Vector3.up * lean);
        }

        protected float ComputeForce(Wheel target)
        {
            float force = 1f;
            if (target.Collider != null && target.Collider.isGrounded)
            {
                WheelHit hit = new WheelHit();
                target.Collider.GetGroundHit(out hit);
                force = ((hit.point - target.Collider.transform.position).magnitude / target.Collider.suspensionDistance);
            }
            return force * owner.target.Traits.antirollForce;
        }
    }
}