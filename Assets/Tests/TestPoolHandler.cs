
using NUnit.Framework;
using UnityEngine;

namespace com.braineeeeDevs.gr.Tests
{
    public class TestPoolHandler
    {
        public PoolHandler handler;

        /// <summary>
        /// Creates one instance of "example" from your Resources folder.
        /// </summary>
        /// <returns></returns>     
        public VehicleComponent CreateObjectWith(string prefabName, System.Guid id)
        {
            var newObj = (MonoBehaviour.Instantiate(Resources.Load(prefabName)) as GameObject).GetComponent<VehicleComponent>();
            newObj.Traits.poolID = id.ToString();
            return newObj;
        }
        [TearDown]
        public void TearDown()
        {
            GameObject.Destroy(handler);
        }
        [SetUp]
        public void Setup()
        {
            handler = GameObject.CreatePrimitive(PrimitiveType.Sphere).AddComponent<PoolHandler>();
        }

        [Test]
        public void TestStoresObject()
        {
            Setup();
            var obj = CreateObjectWith("example", System.Guid.NewGuid());
            PoolHandler.GiveObject(obj);
            Assert.IsTrue(handler.Count != 0, "PoolHandler has failed to add a new pool for given object.");
            TearDown();
        }

        [Test]
        public void TestPoolingSameID()
        {
            Setup();
            var first = CreateObjectWith("example", System.Guid.NewGuid());
            var second = CreateObjectWith("example", new System.Guid(first.Traits.poolID));
            PoolHandler.GiveObject(first);
            PoolHandler.GiveObject(second);
            Assert.IsTrue(handler.GetQuantityOfObjectsPooledByID(second.PoolID) == 2, "PoolHandler has failed to pool objects with the same ID together.");
            TearDown();
        }

        [Test]
        public void TestPoolingDifferentIDs()
        {
            Setup();
            var obj_example = CreateObjectWith("example", System.Guid.Empty);
            var obj_different = CreateObjectWith("different_example", System.Guid.Empty);
            PoolHandler.GiveObject(obj_example);
            PoolHandler.GiveObject(obj_different);
            Assert.IsTrue(handler.Count == 2 && handler.GetQuantityOfObjectsPooledByID(obj_example.PoolID) == 1 && handler.GetQuantityOfObjectsPooledByID(obj_different.PoolID) == 1, "PoolHandler has failed to pool objects with the same ID together.");
            TearDown();
        }
        [Test]
        public void TestRetrieveObject()
        {
            Setup();
            var obj = CreateObjectWith("example", System.Guid.NewGuid());
            PoolHandler.GiveObject(obj);
            var result = PoolHandler.GetObject(obj.PoolID, obj.transform);
            Assert.IsNotNull(result, "PoolHandler has failed to add a new pool for given object.");
            TearDown();
        }


    }
}