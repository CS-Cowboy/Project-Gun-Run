using System.Collections.Generic;
using System;
using UnityEngine;

namespace com.braineeeeDevs.gr
{

    /// <summary>
    /// A class form pooling objects. Ideally you use this for one sort of gameobject at a time (don't mix gameObjects that are different in terms of gameplay but are derived from BasicObject).
    /// Ie. you have an enemy called "Jackyl", and an enemy called "Elefann", both of which derive from BasicObject but are two distinct enemies.   
    /// /// </summary>
    public class ObjectPool : MonoBehaviour
    {
        public uint capacity = 100;
        private Guid id = Guid.Empty;
        protected BasicObject prefab;
        protected Stack<BasicObject> objects = new Stack<BasicObject>();
        public Guid ID
        {
            get
            {
                return id;
            }
        }
        public int Count
        {
            get
            {
                return objects.Count;
            }
        }

        /// <summary>
        /// Gives the pooler an ID and a prefab. It is important to call this at least once somewhere to make the pooler usable.
        /// </summary>
        /// <param name="obj">The object this pool will use.</param>
        public void AssignPrefab(BasicObject obj)
        {
            if (id == Guid.Empty)
            {
                prefab = obj;
                capacity = prefab.traits.poolCapacity;
                id = obj.PoolID;
            }
            else
            {
                Debug.LogWarning("This pool has already been assigned a prefab. Please try creating a new ObjectPool by calling AddComponent<ObjectPool>() on your desired MonoBehaviour object.");
            }
        }
        /// <summary>
        /// Puts an object back into the pool or destroys it if pool is at capacity. Logs a warning if AssignPrefab() has not been called.
        /// </summary>
        /// <param name="obj">The object to put back.</param>
        public void Give(BasicObject obj)
        {
            if (id != Guid.Empty)
            {
                if (objects.Count == capacity)
                {
                    DestroyImmediate(obj);
                }
                else
                {
                    objects.Push(obj);
                }
            }
            else
            {
                Debug.LogWarning(String.Format("The pool has not been assigned a prefab. You must give it a prefab prior to use by calling AssignPrefab()."));
            }

        }

        /// <summary>
        /// Retrieves an object from the pool, or creates one if pool is empty. Returns null if AssignPrefab() has not been called.
        /// </summary>
        /// <returns>A BaseObject instance.</returns>
        public BasicObject GetObject()
        {
            BasicObject obj = null;
            if (id != Guid.Empty)
            {
                if (objects.Count > 0)
                {
                    obj = objects.Pop();
                }
                else
                {
                    obj = Instantiate(prefab).GetComponent<BasicObject>();
                }
            }
            else
            {
                Debug.LogWarning(String.Format("The pool has not been assigned a prefab. You must give it a prefab prior to use by calling AssignPrefab()."));
            }
            return obj;
        }


    }

}