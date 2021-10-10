using System;
using UnityEngine;
namespace com.braineeeeDevs.gr
{
    /// <summary>
    /// An abstract class to represent generic gameobjects which are poolable and require rigidbody physics, sound effects, and animations. Cannot be directly instantiated. You must derive from it to see it exist in game.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Animation))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(AudioSource))]
    public abstract class BasicObject : MonoBehaviour
    {
        public AudioSource sfx;
        public ObjectAttributes traits;
        protected Animation animators;
        protected Rigidbody rbPhysics;
        protected Guid id;
        public Guid PoolID
        {
            get
            {
                return id;
            }
        }
        public virtual void Awake()
        {
            if (traits.poolID == Guid.Empty.ToString() || traits.poolID == String.Empty)
            {
                id = Guid.NewGuid();
                traits.poolID = id.ToString();
            }
            else
            {
                id = new Guid(traits.poolID);
            }
        }
        public virtual void Start()
        {
            animators = GetComponent<Animation>();
            rbPhysics = GetComponent<Rigidbody>();
            sfx = GetComponent<AudioSource>();

            if (traits != null)
            {
                rbPhysics.mass = traits.mass;
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
        public virtual void SpawnAt(Transform point)
        {
            transform.position = point.position;
            transform.rotation = point.rotation;
        }
        /// <summary>
        /// Virtual method. Use to return this object to the pool.
        /// </summary>
        public virtual void ReturnToPool()
        {
            gameObject.SetActive(false);
            PoolHandler.GiveObject(this);
        }
    }
}