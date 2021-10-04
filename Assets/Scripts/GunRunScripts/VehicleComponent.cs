using UnityEngine;

namespace com.braineeeeDevs.gunRun
{

    /// <summary>
    /// A class to attach to any vehicle component to simulate the operation of said vehicle.
    /// </summary>
    /// >
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]

    public abstract class VehicleComponent : MonoBehaviour, ITakeDamage
    {
        /// <summary>
        /// Determined by quantity of hits.
        /// </summary>
        
        public enum THERMAL_CONDITION { COOL = 0, AT_OPERATING_TEMP = 1, HEAT_STRESSED = 2, HEAT_DAMAGED = 3, NON_OPERABLE_TERMINALLY_OVERHEATED = 4};
        public THERMAL_CONDITION currentThermalCondition = THERMAL_CONDITION.COOL;

        public virtual void Start()
        {
            owner = GetComponentInParent<GroundVehicle>();
        }
        public GroundVehicle owner;
        /// <summary>
        /// Used by other components up the input line to drive this component.
        /// </summary>
        /// <param name="delta">The change in the quantity this component is to control.</param>
        public virtual void Operate(float inputChange)
        {
            Debug.Log("Base Operate(dt) Called.");
        }
        public virtual void Operate()
        {

            Debug.Log("Base Operate(void) Called.");
        }

        /// <summary>
        /// Would be used for damage application but a hit based simulation seemed more appropriate. This method has no effect. 
        /// </summary>
        /// <param name="quantity">The quantity of damage to apply.</param>
        void ITakeDamage.ApplyDamage(float quantity)
        {

        }
        /// <summary>
        /// Used to apply damage hits to the component.
        /// </summary>
        void ITakeDamage.Hit()
        {
            switch(currentThermalCondition)
            {
                case THERMAL_CONDITION.COOL:
                    currentThermalCondition = THERMAL_CONDITION.AT_OPERATING_TEMP;
                    break;
                case THERMAL_CONDITION.AT_OPERATING_TEMP:
                    currentThermalCondition = THERMAL_CONDITION.HEAT_STRESSED;
                    break;
                case THERMAL_CONDITION.HEAT_STRESSED:
                    currentThermalCondition = THERMAL_CONDITION.HEAT_DAMAGED;
                    break;
                case THERMAL_CONDITION.HEAT_DAMAGED:
                    currentThermalCondition = THERMAL_CONDITION.NON_OPERABLE_TERMINALLY_OVERHEATED;
                    break;
            };
        }
        /// <summary>
        /// Would be used to repool this component but its not necessary. This method has no effect. 
        /// </summary>
        void ITakeDamage.Die()
        {

        }

    }
}