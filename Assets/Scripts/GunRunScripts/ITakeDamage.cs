using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.braineeeeDevs.gunRun
{
    interface ITakeDamage
    {
        void ApplyDamage(float quantity);
        void Hit();
        void Die();
    }
}