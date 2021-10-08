using UnityEngine;

namespace com.braineeeeDevs.gr
{
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(Calculus))]
    /// <summary>
    /// A class to control the player camera.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        public float downScale = 0.1f;
        public static Transform orbiter;
        public static Controller playerControls;
        public static Camera playerCamera;
        protected Calculus mousePhysics;
        protected Vector3 orbiterRotation;
        public float rollSpeed = 3f, upper = 30f, lookSpeed = 2f, speedMultipler = 4f, lookAcceleration = 2f, camSpeed = 2f, zoomSpeed = 0.5f, innerZoomLimit = 5f, outerZoomLimit = 20f, lowerCamSpeedLimit = 2f, upperCamSpeedLimit = 100f;
        void Awake()
        {
            playerCamera = GetComponent<Camera>();
            playerControls = GetComponent<Controller>();
            mousePhysics = GetComponent<Calculus>();
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
        protected void OrbitCamera(Vector3 lookDelta)
        {
            var verticalTheta = Mathf.Clamp(orbiterRotation.x - lookDelta.y, 0f, upper);
            orbiter.rotation = Quaternion.Euler(verticalTheta, orbiterRotation.y + lookDelta.x, 0f);
            orbiterRotation = orbiter.eulerAngles;
        }
        protected void DollyCamera(float zoomDelta)
        {
            var zoomedPosition = -Mathf.Clamp((zoomDelta * zoomSpeed - transform.localPosition.z), innerZoomLimit, outerZoomLimit);
            playerCamera.transform.localPosition = new Vector3(0f, 0f, zoomedPosition);
            RaycastHit hit;
            Ray ray = new Ray(orbiter.position, -1f * orbiter.forward);
            if (Physics.Raycast(ray, out hit, transform.localPosition.magnitude))
            {
                transform.position = hit.point;
            }
        }
        protected void LookCamera(Vector2 lookDelta, float roll)
        {
            transform.Rotate((Vector3.right * -lookDelta.y + Vector3.up * lookDelta.x + Vector3.forward * roll * rollSpeed), Space.Self);
        }
        /// <summary>
        /// Moves the camera. Prevents it from clipping through objects behind it. Only translates if the player controls puppet is null.
        /// </summary>
        /// <param name="moveDelta">The change in movement desired.</param>
        /// <param name="speedDelta">The change in camera orientation desired,</param>
        protected void TranslateCamera(Vector3 moveDelta, float speedDelta)
        {
            camSpeed = Mathf.Clamp(camSpeed + speedDelta * speedMultipler, lowerCamSpeedLimit, upperCamSpeedLimit);
            transform.Translate(new Vector3(moveDelta.x, 0f, moveDelta.y).normalized * camSpeed * Time.deltaTime);
        }
        protected void FixedUpdate()
        {
            mousePhysics.Compute(playerControls.mousePosition);
        }
        protected void LateUpdate()
        {
            Vector3 lookPhysics = Time.deltaTime * (mousePhysics.ThreeSpacePosition + mousePhysics.ThreeSpaceVelocity * lookSpeed + mousePhysics.ThreeSpaceAcceleration * lookAcceleration);
            if (orbiter != null && CameraController.playerControls.puppet != null)
            {
                OrbitCamera(lookPhysics);
                DollyCamera(lookPhysics.z);
            }
            else
            {
                LookCamera(lookPhysics, playerControls.roll);
                TranslateCamera(playerControls.drive_and_steering, lookPhysics.z);
            }

        }

    }
}
