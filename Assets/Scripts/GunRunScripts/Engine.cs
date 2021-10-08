using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.braineeeeDevs.gr
{
    public class Engine : VehicleComponent
    {
        public float speed = 0f;
        /// <summary>
        /// Operates the Engine. You must call this every FixedUpdate regardless of its condition.
        /// </summary>
        /// <param name="input">The input directly from the Controller in range of [0,1].</param>
        public override void Operate(float input)
        {
            speed = Mathf.Clamp(input, owner.vehicleTraits.idleSpeed, owner.vehicleTraits.tachometerLimit);
            //Don't forget to add sound fx and animation; this is pointless without effects!
        }
    }
}