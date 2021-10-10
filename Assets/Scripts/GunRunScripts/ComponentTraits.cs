using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.braineeeeDevs.gr
{
    [RequireComponent(typeof(AnimationClip))]
    [CreateAssetMenu()]
    public class ComponentTraits : ScriptableObject
    {
        public AnimationCurve hitsCurve;
        public float maxHits = 5f;
    }
}