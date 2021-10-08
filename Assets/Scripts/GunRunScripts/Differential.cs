using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace com.braineeeeDevs.gr
{
    public class Differential : VehicleComponent
    {
        public float left_torque, right_torque;

        /// <summary>
        /// Operates the Differential. You must call this every FixedUpdate regardless of its condition.
        /// </summary>
        /// <param name="inputTorque">The torque (Nm).</param>
        public override void Operate(float inputTorque)
        {
            
            left_torque = right_torque = inputTorque * 0.5f;
        }
    }
}