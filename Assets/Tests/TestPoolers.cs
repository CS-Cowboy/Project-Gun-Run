
using NUnit.Framework;
using UnityEngine;

namespace com.braineeeeDevs.gr.Tests
{
    public class TestPoolers
    {
        public ExampleObject newObject;
        public ObjectPool pool;
        /// <summary>
        /// Loads the pool to capacity.
        /// </summary>
        public void LoadPool()
        {
            for (int c = 0; c < pool.capacity; c++)
            {
                pool.Give(CreateObject()); //Calling the BaseObject method because it automatically disables the object.
            }
        }
        /// <summary>
        /// Creates one instance of "example" from your Resources folder.
        /// </summary>
        /// <returns></returns>
        public ExampleObject CreateObject()
        {
            var exObj = (MonoBehaviour.Instantiate(Resources.Load("example")) as GameObject).GetComponent<ExampleObject>();
            exObj.traits.poolID = System.Guid.Empty.ToString();
            return exObj;
        }
        [TearDown]
        public void TearDown()
        {
            GameObject.Destroy(pool);
            GameObject.Destroy(newObject);
        }
        [SetUp]
        public void Setup()
        {
            newObject = CreateObject();
            pool = GameObject.CreatePrimitive(PrimitiveType.Sphere).AddComponent<ObjectPool>();
            pool.AssignPrefab(newObject);
        }
        // A Test behaves as an ordinary method
        [Test]
        public void TestBasicObjectExists()
        {
            Setup();
            Assert.IsNotNull(newObject, "BaseObject prefab is null. Did you forget to assign a prefab?");
        }
        [Test]
        public void TestPoolHasGuid()
        {
            Setup();
            Assert.IsTrue(pool.ID != System.Guid.Empty, "ObjectPool has not been given an ID. Have you called AssignPrefab?");
        }
        [Test]
        public void TestPoolSizeIncreasedOnReturnObject()
        {
            Setup();
            pool.Give(newObject);
            Assert.IsTrue(pool.Count == 1, "ObjectPool has failed to return object.");
            TearDown();

        }
        [Test]
        public void TestGetMethod()
        {
            Setup();
            newObject = pool.GetObject() as ExampleObject;
            Assert.IsNotNull(newObject, "ObjectPool has failed to retrieve an object.");
            TearDown();

        }
        [Test]
        public void TestSizeHasDecreased()
        {
            Setup();
            pool.Give(newObject);
            var gottenObj = pool.GetObject();
            Assert.IsTrue(pool.Count == 0, "ObjectPool has failed to store and retrieve an object.");
            TearDown();
        }
        [Test]
        public void TestCapacity()
        {
            Setup();
            LoadPool();
            //Do this once more and return to achieve test.
            pool.Give(CreateObject());
            Assert.IsTrue(pool.Count == newObject.traits.poolCapacity, "ObjectPool has failed to limit its capacity.");
            TearDown();

        }
        [Test]
        public void TestPooledObjectWasDestroyedAfterReachingCapacity()
        {   
            Setup();
            LoadPool();
            //Do this once more to achieve test.
            ExampleObject extra = CreateObject();
            pool.Give(extra);
            Assert.IsNotNull(extra, "ObjectPool has failed to destroy the extra object.");
            TearDown();
        }
    }
}