using UnityEngine;

namespace com.braineeeeDevs.gr
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

        public enum DAMAGE_CONDITION { LEVEL_1 = 0, LEVEL_2 = 1, LEVEL_3 = 2, LEVEL_4 = 3, LEVEL_5 = 4, LEVEL_6 = 6,  LEVEL_7 = 7 };
        public DAMAGE_CONDITION currentThermalCondition = DAMAGE_CONDITION.LEVEL_1;

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
            switch (currentThermalCondition)
            {
                case DAMAGE_CONDITION.LEVEL_1:
                    currentThermalCondition = DAMAGE_CONDITION.LEVEL_2;
                    break;
                case DAMAGE_CONDITION.LEVEL_2:
                    currentThermalCondition = DAMAGE_CONDITION.LEVEL_3;
                    break;
                case DAMAGE_CONDITION.LEVEL_3:
                    currentThermalCondition = DAMAGE_CONDITION.LEVEL_4;
                    break;
                case DAMAGE_CONDITION.LEVEL_4:
                    currentThermalCondition = DAMAGE_CONDITION.LEVEL_5;
                    break;
                case DAMAGE_CONDITION.LEVEL_5:
                    currentThermalCondition = DAMAGE_CONDITION.LEVEL_6;
                    break;
                case DAMAGE_CONDITION.LEVEL_6:
                    currentThermalCondition = DAMAGE_CONDITION.LEVEL_7;
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