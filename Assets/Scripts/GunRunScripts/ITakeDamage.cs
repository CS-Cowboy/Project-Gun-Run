using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.braineeeeDevs.gr
{
    interface ITakeDamage
    {
        void Hit();
        float EvaluateHits(AnimationCurve target);
        void Die();
    }
}