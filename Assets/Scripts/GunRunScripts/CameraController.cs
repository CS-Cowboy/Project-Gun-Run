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
        public static CameraController cameraControls;
        public Controller playerControls;
        public Camera playerCamera;
        protected Calculus mousePhysics;
        protected Vector3 orbiterRotation;
        public CameraSettings settings;
        void Awake()
        {
            CameraController.cameraControls = this;
            playerCamera = GetComponent<Camera>();
            playerControls = GetComponent<Controller>();
            mousePhysics = GetComponent<Calculus>();
        }
        public void AttachTo(Body target)
        {
            if (target != null)
            {
                playerControls.AssignTarget(target);
                playerControls.transform.SetParent(target.transform);
            }
            else
            {
                playerControls.target = null;
                playerControls.transform.SetParent(null);
            }
            orbiterRotation = playerControls.target.Orbiter.rotation.eulerAngles;
        }
        /// <summary>
        /// Rotates the camera.
        /// </summary>
        /// <param name="lookDelta">The change in the camera's orientation.</param>         
        protected void OrbitCamera(Vector3 lookDelta)
        {
            var verticalTheta = Mathf.Clamp(orbiterRotation.x - lookDelta.y, 0f, settings.upperVerticalThetaLimit);
            playerControls.target.Orbiter.rotation = Quaternion.Euler(verticalTheta, orbiterRotation.y + lookDelta.x, 0f);
            orbiterRotation = playerControls.target.Orbiter.eulerAngles;
        }
        protected void DollyCamera(float zoomDelta)
        {
            var zoomedPosition = -Mathf.Clamp((zoomDelta * settings.zoomSpeed - transform.localPosition.z), settings.innerZoomLimit, settings.outerZoomLimit);
            playerCamera.transform.localPosition = new Vector3(0f, 0f, zoomedPosition);
            RaycastHit hit;
            Ray ray = new Ray(playerControls.target.Orbiter.position, -1f * playerControls.target.Orbiter.forward);
            if (Physics.Raycast(ray, out hit, transform.localPosition.magnitude))
            {
                transform.position = hit.point;
            }
        }
        protected void LookCamera(Vector2 lookDelta, float roll)
        {
            transform.Rotate((Vector3.right * -lookDelta.y + Vector3.up * lookDelta.x + Vector3.forward * roll * settings.rollSpeed), Space.Self);
        }
        /// <summary>
        /// Moves the camera. Prevents it from clipping through objects behind it. Only translates if the player controls puppet is null.
        /// </summary>
        /// <param name="moveDelta">The change in movement desired.</param>
        /// <param name="speedDelta">The change in camera orientation desired,</param>
        protected void TranslateCamera(Vector3 moveDelta, float speedDelta)
        {
            settings.camSpeed = Mathf.Clamp(settings.camSpeed + speedDelta * settings.speedMultipler, settings.lowerCamSpeedLimit, settings.upperCamSpeedLimit);
            transform.Translate(new Vector3(moveDelta.x, 0f, moveDelta.y).normalized * settings.camSpeed * Time.deltaTime);
        }
        protected void FixedUpdate()
        {
            mousePhysics.Compute(playerControls.mousePosition);
        }
        protected void LateUpdate()
        {
            Vector3 lookPhysics = (mousePhysics.ThreeSpaceVelocity * settings.lookSpeed + mousePhysics.ThreeSpaceAcceleration * settings.lookAcceleration);
            if (playerControls.target.Orbiter != null && playerControls.target != null)
            {
                OrbitCamera(Time.deltaTime * (lookPhysics));
                DollyCamera(lookPhysics.z);
            }
            else
            {
                LookCamera(Time.deltaTime * (lookPhysics), Time.deltaTime * playerControls.roll);
                TranslateCamera(Vector3.right * playerControls.steering + Vector3.forward * playerControls.driving, lookPhysics.z);
            }

        }

    }
}
