using System.Collections.Generic;
using UnityEngine;
using System;

namespace com.braineeeeDevs.gr
{
    /// <summary>
    /// A class for handling all ObjectPools. The methods are static, meaning they are global and to use them you must use this syntax: "PoolController.Method()", where Method() belongs to this class.
    /// In short, you do not need to handle the lifecyle of your ObjectPools. This will assign each prefab to its rightful pool according to its Guid (read: don't mess with the Guid at runtime unless you want problems).
    /// Note: The Guid is generated at runtime and will always be different when you run your game.
    ///  </summary>
    public class PoolHandler : MonoBehaviour
    {
        protected static Dictionary<Guid, ObjectPool> pools = new Dictionary<Guid, ObjectPool>();

        /// <summary>
        /// The quantity of pools held internally.
        /// </summary>
        /// <value>The number of pools in existence.</value>
        public int Count
        {
            get
            {
                return pools.Count;
            }
        }
        /// <summary>
        /// Gets a truth value for whether this class has an ID.
        /// </summary>
        /// <param name="id">The Guid to check for.</param>
        /// <returns>True if it exists internally, false otherwise.</returns>
        public bool HasID(Guid id)
        {
            return pools.ContainsKey(id);
        }
        /// <summary>
        /// Gets the number of objects pooled with a particular ID. 
        /// </summary>  
        /// <param name="id">The ID to of object types to find.</param>
        /// <returns>The quantity of objects pooled with the ID given.</returns>
        public int GetQuantityOfObjectsPooledByID(Guid id)
        {
            return pools[id].Count;
        }
        /// <summary>
        /// Used to determine if a pool has any copies of a particular object based on its Guid.
        /// </summary>
        /// <param name="id">The Guid to search for.</param>
        /// <returns>True if not empty, false if empty.</returns>
        public bool PoolIsNotEmpty(Guid id)
        {
            return pools[id].Count > 0;
        }
        /// <summary>
        /// Returns a BasicObject derived object to it associated pool.
        /// </summary>
        /// <param name="obj">The object to put away.</param>
        public static void GiveObject(BasicObject obj)
        {
            if (obj.PoolID != Guid.Empty)
            {
                if (pools.ContainsKey(obj.PoolID))
                {
                    pools[obj.PoolID].Give(obj);
                }
                else
                {
                    CreateNewPoolFor(obj);
                }
            }
            else
            {
                Debug.LogWarning("Object given to 'GiveObject()' has an empty Guid.");
            }
        }
        /// <summary>
        /// Creates the pool for a new object and puts the new object in it.
        /// </summary>
        /// <param name="obj">The object to generate a new pool for.</param>                
        protected static void CreateNewPoolFor(BasicObject obj)
        {
            var newPool = GameObject.CreatePrimitive(PrimitiveType.Sphere).AddComponent<ObjectPool>();
            newPool.GetComponent<MeshRenderer>().enabled = false; //Force invisible objectPooler. Don't want to distract our gamers with random spheres appearing!
            newPool.AssignPrefab(obj);
            newPool.Give(obj);
            pools.Add(newPool.ID, newPool);
        }
        /// <summary>
        /// Retrieves an object based on the object's Guid. You need to give your prefabs to this object at runtime to have a pool associated with your object. 
        /// </summary>
        /// <param name="id">The Guid of the object you wish to get a clone of.</param>
        /// <returns>An object from the associated pool. Null if this class has never been given an instance of the desired object.</returns>
        public static BasicObject GetObject(Guid id)
        {
            if (id != Guid.Empty)
            {
                if (pools.ContainsKey(id))
                {
                    return pools[id].GetObject();
                }
                else
                {
                    Debug.LogWarning("Object does not exist in the global pool. Have you called ReturnObject() at least once?");
                    return null;
                }
            }
            else
            {
                Debug.LogWarning("Guid used to retrieve object via 'GetObject()' has an empty Guid.");
                return null;
            }
        }
    }

}