using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace com.braineeeeDevs.gunRun
{
    public class Differential : VehicleComponent
    {
        public float left_torque, right_torque;

        public override void Operate(float inputTorque)
        {
            inputTorque *= 0.5f;
            left_torque = right_torque = inputTorque;
        }
    }
}