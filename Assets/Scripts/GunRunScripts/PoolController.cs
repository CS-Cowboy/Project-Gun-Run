using System.Collections.Generic;
using UnityEngine;
using System;

namespace com.braineeeeDevs.gr
{
    public class PoolController : MonoBehaviour
    {
        public static Dictionary<Guid, ObjectPool> pools = new Dictionary<Guid, ObjectPool>();
        public static GameObject instantiable;
        public void Awake()
        {
            if (PoolController.instantiable == null)
            {
                instantiable = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            }
        }
        public static BasicObject GetObject(Guid id)
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
        public static void ReturnObject(BasicObject obj)
        {
            if (pools.ContainsKey(obj.originPoolID))
            {
                pools[obj.originPoolID].Return(obj);
            }
            else
            {
                CreateNewPoolFor(obj);
            }
        }

        public static void CreateNewPoolFor(BasicObject obj)
        {
            var newPool = Instantiate(instantiable).AddComponent<ObjectPool>();
            newPool.AssignPrefab(obj);
            newPool.capacity = obj.traits.poolCapacity;
            pools.Add(newPool.ID, newPool);
        }

        public static string[] GetNamesAssociatedWith(Guid sourceID)
        {
            string[] names;
            if (pools.ContainsKey(sourceID))
            {
                uint c = 0;
                names = new string[pools[sourceID].uniqueCountedObjectNames.Keys.Count];
                foreach (string k in pools[sourceID].uniqueCountedObjectNames.Keys)
                {
                    names[c] = k;
                    c++;
                }
                return names;
            }
            else
            {
                Debug.LogWarning("The global object pool does not contain a GUID with this value. Have you tried adding a new prefab to it by calling ReturnObject() with your prefab?");
                return new string[0];
            }
        }

    }
}