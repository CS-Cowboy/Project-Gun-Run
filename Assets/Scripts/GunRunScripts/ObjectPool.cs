using System.Collections.Generic;
using System;
using UnityEngine;

namespace com.braineeeeDevs.gr
{
    public class ObjectPool : MonoBehaviour
    {
        public uint capacity = 100;
        private Guid id = Guid.Empty;
        public Dictionary<string, uint> uniqueCountedObjectNames = new Dictionary<string, uint>();
        [SerializeField] protected BasicObject prefab;
        protected Stack<BasicObject> objects = new Stack<BasicObject>();
        public Guid ID
        {
            get
            {
                return id;
            }
        }
        public void Return(BasicObject obj)
        {
            if (id != Guid.Empty)
            {
                if ((objects.Count + 1) >= capacity)
                {
                    if (uniqueCountedObjectNames.ContainsKey(obj.name))
                    {
                        uniqueCountedObjectNames[obj.name] -= 1;
                        if (uniqueCountedObjectNames[obj.name] == 0)
                        {
                            uniqueCountedObjectNames.Remove(obj.name);
                        }
                    }
                    DestroyImmediate(obj);
                }
                else
                {
                    if (!uniqueCountedObjectNames.ContainsKey(obj.name))
                    {
                        uniqueCountedObjectNames.Add(obj.name, 1);
                    }
                    else
                    {
                        uniqueCountedObjectNames[obj.name] += 1;
                    }
                    objects.Push(obj);
                }
            }
            else
            {
                Debug.LogWarning(String.Format("The pool has not been assigned a prefab. You must give it a prefab prior to use by calling AssignPrefab()."));
            }

        }

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

        public void AssignPrefab(BasicObject obj)
        {
            if (id == Guid.Empty)
            {
                id = new Guid();
            }
            else
            {
                Debug.LogWarning("This pool has already been assigned a prefab. Please try creating a new ObjectPool by calling AddComponent<ObjectPool>() on your desired MonoBehaviour object.");
            }
        }

    }

}