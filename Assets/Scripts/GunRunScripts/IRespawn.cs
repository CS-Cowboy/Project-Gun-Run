using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace com.braineeeeDevs.gunRun
{
    interface IRespawn
    {
        void Spawn(Transform point);
        void ReturnToPool();
    }
}