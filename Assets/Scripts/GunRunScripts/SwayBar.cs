
using UnityEngine;

namespace com.braineeeeDevs.gr
{
    public class SwayBar : VehicleComponent
    {
        public Wheel left, right;

        ITakeDamage damage;
        public void Awake()
        { damage = this as ITakeDamage; }
        /// <summary>
        /// Computes the swaybar force to prevent vehicle roll.
        /// </summary>
        /// <returns>The anti-roll force.</returns>
        public override void Operate()
        {
            var lean = owner.SteeringAndDrive.x < 0f ? (ComputeForce(right) - ComputeForce(left)) : (ComputeForce(left) - ComputeForce(right));
            right.antiRollForce = left.antiRollForce = lean * damage.EvaluateHits(owner.vehicleTraits.components.swaybarHitsCurve);
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
            return force * owner.vehicleTraits.antiRollForce;
        }
    }
}