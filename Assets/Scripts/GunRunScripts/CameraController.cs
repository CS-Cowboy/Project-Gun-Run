using UnityEngine;

namespace com.braineeeeDevs.gunRun
{
    [RequireComponent(typeof(Camera))]

    [RequireComponent(typeof(Calculus))]
    [RequireComponent(typeof(Calculus))]
    /// <summary>
    /// A class to control the player camera.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        public static Transform orbiter;
        public static Controller playerControls;
        public static Camera playerCamera;
        protected Calculus mousePhysics, cameraPhysics;
        protected bool inRange = false;
        protected Vector3 orbiterRotation;
        public float lower = 5f, upper = 70f, lookSpeed = 10f, lookAcceleration = 5f, camAcceleration = 2f, camSpeed = 2f, zoomSpeed = 20f, innerZoomLimit = 5f, outerZoomLimit = 20f, upperCamSpeedLimit = 100f;
        void Awake()
        {
            playerCamera = GetComponent<Camera>();
            playerControls = GetComponent<Controller>();
            mousePhysics = GetComponent<Calculus>();
            cameraPhysics = GetComponent<Calculus>();
        }
        private void Start()
        {
            orbiterRotation = playerControls.puppet.orbiter.rotation.eulerAngles;
        }
        /// <summary>
        /// Attaches the player camera and controls to the GroundVehicle.
        /// </summary>
        /// <param name="puppet">The GroundVehicle to attach and control.</param>
        public static void AttachTo(GroundVehicle puppet)
        {
            if (puppet != null)
            {
                playerControls.SetPuppet(puppet);
                playerCamera.transform.SetParent(puppet.orbiter);
                orbiter = puppet.orbiter;
            }
            else
            {

                playerCamera.transform.SetParent(null);
                playerControls.SetPuppet(null);
                orbiter = null;
            }
        }
        /// <summary>
        /// Rotates the camera.
        /// </summary>
        /// <param name="lookDelta">The change in the camera's orientation.</param>      
        protected void RotateCamera(Vector3 lookDelta)
        {
            lookDelta *= Time.fixedDeltaTime;
            var thetaV = Mathf.Clamp(orbiterRotation.x - lookDelta.y, lower, upper);
            orbiter.rotation = Quaternion.Euler(thetaV, orbiterRotation.y + lookDelta.x, 0f);
        }
        /// <summary>
        /// Moves the camera. Prevents it from clipping through objects behind it. Only translates if the player controls puppet is null.
        /// </summary>
        /// <param name="moveDelta">The change in movement desired.</param>
        /// <param name="zoomDelta">The change in camera orientation desired,</param>
        protected void TranslateCamera(Vector3 moveDelta, float zoomDelta)
        {
            moveDelta *= Time.deltaTime;
            if (playerControls.puppet != null)
            {
                var zoomedPosition = -Mathf.Clamp((zoomDelta * zoomSpeed - playerCamera.transform.localPosition.z), innerZoomLimit, outerZoomLimit);
                playerCamera.transform.localPosition = new Vector3(0f, 0f, zoomedPosition);
            }
            else
            {
                playerCamera.transform.Translate(new Vector3(moveDelta.x, 0f, moveDelta.y));
            }
            RaycastHit hit;
            Ray ray = new Ray(orbiter.position, -1f * orbiter.forward);
            if (Physics.Raycast(ray, out hit, playerCamera.transform.localPosition.magnitude))
            {
                playerCamera.transform.position = hit.point + orbiter.forward;
            }
        }
        protected void FixedUpdate()

        {
            cameraPhysics.Compute(playerControls.drive_and_steering);
            mousePhysics.Compute(playerControls.mousePosition);
        }
        protected void LateUpdate()
        {
            var lookPhysics = mousePhysics.Velocity * lookSpeed + mousePhysics.Acceleration * lookAcceleration;
            var movePhysics = cameraPhysics.Position + cameraPhysics.Velocity * camSpeed + cameraPhysics.Acceleration * camAcceleration;
            RotateCamera(lookPhysics);
            TranslateCamera(movePhysics, mousePhysics.Position.z);
        }

    }
}
