using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class MathUtilities
{
    public static float horsepower2Watts = 745.7f;
    public const uint wheelQuantity = 4;
    public static float metersPerSecToKPH = 3600f * 0.001f;

    public static float MetricTopSpeedWithDrag(float horsepower, float drag)
    {
        return Mathf.Pow(2f * horsepower * horsepower2Watts / drag, Mathf.Exp(1f / 3f));
    }

    public static float LogisticSigmoid(float L, float k, float value)
    {
        return L / (1f + Mathf.Pow(-k * value, Mathf.Exp(0f)));
    }
}
