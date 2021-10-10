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

        public float hits = 0f;
        public ComponentTraits componentTraits;
    
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

        float ITakeDamage.EvaluateHits()
        {
            return Mathf.Clamp(componentTraits.hitsCurve.Evaluate(hits),0f, 1f);
        }
        /// <summary>
        /// Used to apply damage hits to the component.
        /// </summary>
        void ITakeDamage.Hit()
        {
            hits = Mathf.Clamp(hits, 0f, componentTraits.maxHits);
        }
        /// <summary>
        /// Would be used to repool this component but its not necessary. This method has no effect. 
        /// </summary>
        void ITakeDamage.Die()
        {
            hits = componentTraits.maxHits;
        }

    }
}