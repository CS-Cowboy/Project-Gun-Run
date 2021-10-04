using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.braineeeeDevs.gunRun
{
    public class SwayBar : VehicleComponent
    {
        public Wheel left, right;
        public float force;

        /// <summary>
        /// Computes the swaybar force to prevent vehicle roll.
        /// </summary>
        /// <returns>The anti-roll force.</returns>
        public override void Operate()
        {
            force = owner.steeringAngle > 0f ? ComputeForce(left) - ComputeForce(right) : ComputeForce(right) - ComputeForce(left);
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
            return force * target.wheelCollider.sprungMass;
        }
    }
}