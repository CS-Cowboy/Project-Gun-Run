using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.braineeeeDevs.gunRun
{
    public class SwayBar : VehicleComponent
    {
        public Wheel left, right;

        /// <summary>
        /// Computes the swaybar force to prevent vehicle roll.
        /// </summary>
        /// <returns>The anti-roll force.</returns>
        public override void Operate()
        {
            var leanLeft = ComputeForce(left) - ComputeForce(right);
            var leanRight = ComputeForce(right) - ComputeForce(left);
            left.antiRollForce = owner.SteeringAndDrive.x > 0f ? leanLeft : leanRight;
            right.antiRollForce = owner.SteeringAndDrive.x > 0f ? leanRight : leanLeft;
        }

        protected float ComputeForce(Wheel target)
        {
            float force = 1f;
            if (target.wheelCollider.isGrounded)
            {
                WheelHit hit = new WheelHit();
                target.wheelCollider.GetGroundHit(out hit);
                force = ((hit.point - target.wheelCollider.transform.position).magnitude / target.wheelCollider.suspensionDistance) ;
            }
            return force * owner.traits.antiRollForce;
        }
    }
}