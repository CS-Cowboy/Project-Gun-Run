using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.braineeeeDevs.gunRun
{
    [CreateAssetMenu()]
    [RequireComponent(typeof(AnimationCurve))]
    public class CharacterAttributes : ScriptableObject
    {
        public float damagePoints = 0f, armorPoints = 0f, walkSpeed, sprintSpeed, massInKg = 10f;
    }
}