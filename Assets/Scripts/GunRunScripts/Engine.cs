using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.braineeeeDevs.gunRun
{
    public class Engine : VehicleComponent
    {
        public float speed = 0f, deltaAcceleratorRPM = 200f, driveDirection = +1f;
        public override void Operate(float delta)
        {
            driveDirection = delta >= 0f ? +1f : -1f;
            speed = Mathf.Clamp(speed +  delta * deltaAcceleratorRPM, 500f, owner.traits.tachometerLimitInRPM);
            //Don't forget to add sound fx and animation; this is pointless without effects!
        }
    }
}