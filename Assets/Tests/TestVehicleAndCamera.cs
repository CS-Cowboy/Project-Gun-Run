 using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
namespace com.braineeeeDevs.gunRun
{
    /*Tests player and vehicle*/
    public class TestVehicleAndCamera
    {
        public GroundVehicle grndVehicle;
        public Controller controls; 
        public CameraController cameraControls;

        public GameObject[] CreatePlayer()
        {
            var vehicle = MonoBehaviour.Instantiate(Resources.Load<GameObject>("player_prefab").gameObject);
            var cam = MonoBehaviour.Instantiate(Resources.Load<GameObject>("player_view").gameObject);
            return new GameObject[2] { vehicle, cam };
        }
        // A Test behaves as an ordinary method
        [Test]
        public void TestGroundVehicleIsNotNull()
        {
            Assert.NotNull(grndVehicle);
        }

        [Test]
        public void TestGroundVehicleIsActiveAndEnabled()
        {
            Assert.IsTrue(grndVehicle.gameObject.activeSelf);

        }
        [Test]
        public void TestGroundVehicleTraitsIsNotNull()
        {
            Assert.NotNull(grndVehicle.traits);

        }
        [Test]
        public void TestGroundVehicleOrbiterIsNotNull()
        {
            Assert.NotNull(grndVehicle.orbiter);

        }
        [Test]
        public void TestControlsAreNotNull()
        {
            Assert.NotNull(controls);

        }

        [Test]
        public void TestControllerPuppetIsNotNull()
        {
            CameraController.AttachTo(grndVehicle);
            Assert.NotNull(controls.puppet);
            FullReset();
        }


        [Test]
        public void TestCameraControllerIsNotNull()
        {
            Assert.NotNull(cameraControls);

        }

        [Test]
        public void TestCameraControllerOrbiterIsNotnull()
        {
            Assert.NotNull(CameraController.orbiter);

        }
        [Test]
        public void TestCameraControllerPlayerViewIsNotNull()
        {
            Assert.NotNull(CameraController.playerCamera);

        }
        [Test]
        public void TestCameraControllerPlayerControlsAreNotNull()
        {
            Assert.NotNull(CameraController.playerControls);

        }

        [Test]
        public void TestCameraControllerAttachesToGroundVehicle()
        {
            CameraController.AttachTo(null);
            CameraController.AttachTo(grndVehicle);
            Assert.AreEqual(CameraController.playerControls.puppet, grndVehicle);
            FullReset();
        }

        [Test]
        public void TestGroundVehicleWheelsDSIsFull()
        {
            Assert.AreEqual(grndVehicle.wheels.Length, MathUtilities.wheelQuantity);
        }
        [Test]
        public void TestGroundVehicleWheelsAreNotNull()
        {
            for (int c = 0; c < grndVehicle.wheels.Length; c++)
            {
                Assert.NotNull(grndVehicle.wheels[c], string.Format("{0}'th wheel is null", c));
            }
        }
        [Test]
        public void TestGroundVehicleWheelsMeshColliderIsNotNull()
        {
            for (int c = 0; c < grndVehicle.wheels.Length; c++)
            {
                Assert.NotNull(grndVehicle.wheels[c].wheelCollider, string.Format("{0}'th wheel collider is null", c));
            }
        }

        [Test]
        public void TestGroundVehicleWheelsMeshRendererIsNotNull()
        {
            for (int c = 0; c < grndVehicle.wheels.Length; c++)
            {
                Assert.NotNull(grndVehicle.wheels[c].mesh, string.Format("{0}'th wheel mesh renderer is null", c));
            }
        }

        [Test]
        public void TestGroundVehiclesWheelsOpposingTiresAreEqual()
        {
            //Broken currently.
        }

        [Test]
        public void TestGroundVehiclesWheelsOwnerIsNotNull()
        {
            for (int c = 0; c < grndVehicle.wheels.Length; c++)
            {
                Assert.NotNull(grndVehicle.wheels[c].owner, string.Format("{0}'th wheel's owner is null", c));
            }
        }

        public void FullReset()
        {
            TearDown();
            Setup();
        }


        [TearDown]
        public void TearDown()
        {
            GameObject.Destroy(grndVehicle.gameObject);
            GameObject.Destroy(cameraControls.gameObject);
        }
        [SetUp]
        public void Setup()
        {
            var vehicle = CreatePlayer();
            if (vehicle[0] != null)
            {
                grndVehicle = vehicle[0].GetComponent<GroundVehicle>();
            }
            if (vehicle[1] != null)
            {
                controls = vehicle[1].GetComponent<Controller>();
                cameraControls = vehicle[1].GetComponent<CameraController>();
            }
        }
    }

}