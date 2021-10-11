using UnityEngine;
using System;
namespace com.braineeeeDevs.gr
{
    /// <summary>
    /// A class to attach to any vehicle component to simulate the operation of said vehicle.
    /// </summary>
    /// >
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(Animation))]

    public abstract class VehicleComponent : MonoBehaviour
    {
        [SerializeField] protected Controller owner;
        protected float hits = 0f, effectiveness = 1f;
        [SerializeField] protected ComponentTraits componentTraits;

        protected Animation animators;
        protected Guid id;
        public Guid PoolID
        {
            get
            {
                return id;
            }
        }
        public ComponentTraits Traits
        {
            get
            {
                return componentTraits;
            }
            set
            {
                componentTraits = value;
            }
        }
        /// <summary>
        /// Called before Start(). Make sure to call this if you override it, it performs important tasks.
        /// </summary>
        public virtual void Start()
        {
            owner = CameraController.cameraControls.playerControls;
            var m = GetComponent<MeshFilter>();
            if (m.mesh == null) //Just in case.
            {
                m.mesh = componentTraits.mesh;
            }

            if (componentTraits.poolID == Guid.Empty.ToString() || componentTraits.poolID == String.Empty)
            {
                id = Guid.NewGuid();
                componentTraits.poolID = id.ToString();
            }
            else
            {
                id = new Guid(componentTraits.poolID);
            }
            animators = GetComponent<Animation>();
        }
        /// <summary>
        /// Used by other components up the input line to drive this component. Always call this on your main object from your controller.
        /// </summary>
        /// <param name="delta">The change in the quantity this component is to control.</param>
        public virtual void Operate(Vector3 inputChange)
        {
            EvaluateHits();
        }

        public virtual void Operate(float input)
        {
            EvaluateHits();
        }

        public virtual void Operate()
        {
            EvaluateHits();
        }

        /// <summary>
        /// Virtual method. Use for playing an animation.
        /// </summary>
        /// <param name="effectName">The name (verbatim) of the effect.</param>
        public virtual void Play(string effectName) { }

        /// <summary>
        /// Virtual method. Use to return this object to the pool.
        /// </summary>
        public virtual void ReturnToPool()
        {
            gameObject.SetActive(false);
            PoolHandler.GiveObject(this);
        }

        float EvaluateHits()
        {
            return Mathf.Clamp(Traits.hitsCurve.Evaluate(hits), 0f, 1f);
        }
        /// <summary>
        /// Used to apply damage hits to the component.
        /// </summary>
        void Hit()
        {
            hits = Mathf.Clamp(hits, 0f, componentTraits.maxHits);
        }
        /// <summary>
        /// Would be used to repool this component but its not necessary. This method has no effect. 
        /// </summary>
        void Die()
        {
            ReturnToPool();
        }

    }
}