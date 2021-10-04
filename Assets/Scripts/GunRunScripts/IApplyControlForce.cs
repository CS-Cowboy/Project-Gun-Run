using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace com.braineeeeDevs.gunRun
{
    interface IApplyControlForce
    {
        void ApplyControls(Vector3 centerOfMass, float vectorDirection);

        void ApplyControls(Vector3 centerOfMass, float vectorDirection, float vectorTurnDirection);
    }
  

}