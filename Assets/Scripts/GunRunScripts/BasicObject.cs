using System.Collections.Generic;
using UnityEngine;
namespace com.braineeeeDevs.gunRun
{
    /// <summary>
    /// A class to represent generic gameobjects which are poolable and require rigidbody physics and animations.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Animation))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]

    public abstract class BasicObject : MonoBehaviour
    {
        public ObjectAttributes traits;
        protected Animation animators;
        protected Rigidbody rbPhysics;
        
        public virtual void Start()
        {
            animators = GetComponent<Animation>();
            rbPhysics = GetComponent<Rigidbody>();
            
            if (traits != null)
            {   
                rbPhysics.mass = traits.massInKg;
                rbPhysics.angularDrag = rbPhysics.drag = traits.drag;
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
        /// <summary>
        /// Virtual method. Use for playing an animation.
        /// </summary>
        /// <param name="effectName">The name (verbatim) of the effect.</param>
        public virtual void Play(string effectName) { }
        /// <summary>
        /// Virtual method. Use to spawn this object at a particular place in the world. 
        /// </summary>
        /// <param name="point">The transform representing the orientation and position to spawn at in world space.</param>
        public virtual void SpawnAt(Transform point) { }
        /// <summary>
        /// Virtual method. Use to return this object to the pool.
        /// </summary>
        public virtual void ReturnToPool() { }
    }
}