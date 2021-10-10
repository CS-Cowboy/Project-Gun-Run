using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace com.braineeeeDevs.gr
{
    public class Differential : VehicleComponent
    {
        public float left_torque, right_torque;
        ITakeDamage damage;
        public void Awake()
        {
            damage = this as ITakeDamage;
        }
        /// <summary>
        /// Operates the Differential. You must call this every FixedUpdate regardless of its condition.
        /// </summary>
        /// <param name="inputTorque">The torque (Nm).</param>
        public override void Operate(float inputTorque)
        {
            var damageRatio = damage.EvaluateHits();
            left_torque = right_torque = damageRatio * inputTorque * 0.5f;
        }
    }
}