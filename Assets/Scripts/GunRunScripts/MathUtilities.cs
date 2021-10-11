using UnityEngine;
public static class MathUtilities
{
    public static float metersPerSecToKPH = 0.0036f, Nm2Kwatts = 1000.0f * 9.5488f;/*  */
    public const int wheelQuantity = 4;

    /// <summary>
    /// Returns the computed rate of revolution (rev/min).
    /// </summary>
    /// <param name="torque">The rotational force (Nm) desired to convert (rev/min).</param>
    /// <param name="power">The power in (watts) used in producing the torque.</param>
    /// <returns>The RPM computed from the rotational force (Nm) and power (watts).</returns> 

    public static float GetSpeedFrom(float torque, float power)
    {
        //eqn: RPM (rev/min) = 9.5488 x Power (watts) / Torque (Nm)
        return power * Nm2Kwatts / Mathf.Clamp(torque, 1f, float.MaxValue);
    }

    /// <summary>
    /// Returns the computed Torque (Nm).
    /// </summary>
    /// <param name="speed">The speed of revolution in rev/min desired to convert to torque (Nm).</param>
    /// <param name="power">The power in (watts) used in producing the torque.</param>
    /// <returns>The Torque computed from the rotational speed (rev/min) and power (watts).</returns> 

    public static float GetTorqueFrom(float speed, float power)
    {
        //eqn: Torque (N.m) = 9.5488 x Power (watts) / Speed (RPM)
        return power * Nm2Kwatts / Mathf.Clamp(speed, 500f, float.MaxValue);
    }

    /// <summary>
    /// Returns the power (watts).
    /// </summary>
    /// <param name="speed">The speed (rev/min) used to compute the power.</param>
    /// <param name="torque">The torque (Nm) used to compute the power.</param>
    /// <returns>The power (watts) computed from the rotational speed (rev/min) and power (watts).</returns> 

    public static float GetPowerFrom(float speed, float torque)
    {
        //eqn: Power (watts) = Torque (N.m) * Speed (RPM) / 9.5488 
        return speed * torque / Nm2Kwatts;
    }
    public static float MetricTopSpeedWithDrag(float horsepower, float drag)
    {
        return Mathf.Pow(2f * horsepower / drag, Mathf.Exp(1f / 3f));
    }

    public static float LogisticSigmoid(float L, float k, float value)
    {
        return L / (1f + Mathf.Pow(-k * value, Mathf.Exp(0f)));
    }
}
