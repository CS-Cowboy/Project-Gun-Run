
using NUnit.Framework;
using UnityEngine;
namespace com.braineeeeDevs.gr.Tests
{
    /*Tests player and vehicle*/
    public class TestVehicleAndCamera
    {
        public Body groundVehicle;
        public Controller controls;
        public CameraController cameraControls;

        public GameObject[] CreatePlayer()
        {
            var vehicle = MonoBehaviour.Instantiate(Resources.Load("Ford-f-150(Clone) Variant")) as GameObject;
            var cam = MonoBehaviour.Instantiate(Resources.Load("test_player_view")) as GameObject;
            return new GameObject[2] { vehicle, cam };
        }
        // A Test behaves as an ordinary method
        [Test]
        public void TestVehicleComponentIsNotNull()
        {
            Assert.IsNotNull(groundVehicle);
        }

        [Test]
        public void TestVehicleComponentIsActiveAndEnabled()
        {
            Assert.IsTrue(groundVehicle.gameObject.activeSelf);

        }
        [Test]
        public void TestVehicleComponentTraitsIsNotNull()
        {
            Assert.IsNotNull(groundVehicle.Traits);

        }
        [Test]
        public void TestVehicleComponentOrbiterIsNotNull()
        {
            Assert.IsNotNull(groundVehicle.Orbiter);

        }
        [Test]
        public void TestControlsAreNotNull()
        {
            Assert.IsNotNull(controls);

        }

        [Test]
        public void TestControllerPuppetIsNotNull()
        {
            CameraController.cameraControls.AttachTo(groundVehicle);
            Assert.IsNotNull(controls.target);
            FullReset();
        }


        [Test]
        public void TestCameraControllerIsNotNull()
        {
            Assert.IsNotNull(cameraControls);

        }

        [Test]
        public void TestCameraControllerOrbiterIsNotnull()
        {

        }
        [Test]
        public void TestCameraControllerPlayerViewIsNotNull()
        {
            Assert.IsNotNull(CameraController.cameraControls.playerCamera);

        }
        [Test]
        public void TestCameraControllerPlayerControlsAreNotNull()
        {
            Assert.IsNotNull(CameraController.cameraControls.playerControls);

        }

        [Test]
        public void TestCameraControllerAttachesToVehicleComponent()
        {
            CameraController.cameraControls.AttachTo(null);
            CameraController.cameraControls.AttachTo(groundVehicle);
            Assert.AreEqual(CameraController.cameraControls.playerControls.target, groundVehicle);
            FullReset();
        }

        public void FullReset()
        {
            TearDown();
            Setup();
        }


        [TearDown]
        public void TearDown()
        {
            GameObject.Destroy(groundVehicle.gameObject);
            GameObject.Destroy(cameraControls.gameObject);
        }
        [SetUp]
        public void Setup()
        {
            var vehicle = CreatePlayer();
            if (vehicle[0] != null)
            {
                groundVehicle = vehicle[0].GetComponent<Body>();
            }
            if (vehicle[1] != null)
            {
                controls = vehicle[1].GetComponent<Controller>();
                cameraControls = vehicle[1].GetComponent<CameraController>();
            }
        }
    }

}