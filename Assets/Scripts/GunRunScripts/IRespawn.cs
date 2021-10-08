using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace com.braineeeeDevs.gr
{
    interface IRespawn
    {
        void Spawn(Transform point);
        void ReturnToPool();
    }
}