using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.braineeeeDevs.gunRun
{
    public class Engine : VehicleComponent
    {
        Coroutine engineInitializer = null;
        Transmission tran;
        public float speed = 0f;
        public override void Start()
        {
            base.Start();
            tran = owner.transmission;
        }

        public override void Operate(float delta)
        {
            speed = delta * owner.traits.tachometerLimitInRPM;
            //Don't forget to add sound fx and animation; this is pointless without effects!
        }
    }
}