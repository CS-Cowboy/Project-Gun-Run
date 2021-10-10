
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
            right.antiRollForce = left.antiRollForce = lean * damage.EvaluateHits();
        }

        protected float ComputeForce(Wheel target)
        {
            float force = 1f;
            if (target.wheelCollider.isGrounded)
            {
                WheelHit hit = new WheelHit();
                target.wheelCollider.GetGroundHit(out hit);
                force = ((hit.point - target.wheelCollider.transform.position).magnitude / target.wheelCollider.suspensionDistance);
            }
            return force * owner.vehicleTraits.antiRollForce;
        }
    }
}