using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.braineeeeDevs.gunRun
{
    interface IPlayEffects
    {
        void Play(string effectName, float volume);
    }
}