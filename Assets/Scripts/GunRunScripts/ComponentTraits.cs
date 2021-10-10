
using UnityEngine;

namespace com.braineeeeDevs.gr
{
    [RequireComponent(typeof(AnimationClip))]
    [RequireComponent(typeof(AnimationClip))]
    [RequireComponent(typeof(AnimationClip))]
    [RequireComponent(typeof(AnimationClip))]
    [RequireComponent(typeof(AnimationClip))]


    [CreateAssetMenu()]
    public class ComponentTraits : ScriptableObject
    {

        public float maxHits = 5f;
        public AnimationCurve engineHitsCurve;
        public AnimationCurve transmissionHitsCurve;
        public AnimationCurve diffHitsCurve;
        public AnimationCurve wheelHitsCurve;
        public AnimationCurve swaybarHitsCurve;
    }
}