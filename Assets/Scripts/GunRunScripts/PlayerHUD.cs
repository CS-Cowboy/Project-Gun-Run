using UnityEngine;
namespace com.braineeeeDevs.gr
{
    public class PlayerHUD : MonoBehaviour
    {
        //Use cases: a. race position(S)
        //b. speed
        //c. acceleration/tachometer
        //d. race time
        //e. map!

        public UnityEngine.UI.Slider speedometer, tachometer;
        protected UnityEngine.UI.Text lapTime;
        protected float lapTimeCounter;
        public Canvas canvasInstance;
        public CameraSettings camSettings;
        void Start()
        {
            this.BuildHUD();
        }
        /// <summary>
        /// Builds the Heads-Up-Display from the ObjectAttributes 'HudPrefab' variable and attaches it to the player's camera. 
        /// </summary>  
        void BuildHUD()
        {
            if (CameraController.cameraControls.playerControls.target != null)
            {
                var hudObj = Instantiate(CameraController.cameraControls.playerControls.target.Traits.hudPrefab);
                canvasInstance = hudObj.GetComponent<Canvas>();
                canvasInstance.renderMode = camSettings.renderMode;
                canvasInstance.worldCamera = CameraController.cameraControls.playerCamera;
                CameraController.cameraControls.playerCamera.fieldOfView = camSettings.cameraAngle;
                CameraController.cameraControls.playerCamera.usePhysicalProperties = false;
                CameraController.cameraControls.playerCamera.focalLength = camSettings.focusLength;
                CameraController.cameraControls.playerCamera.farClipPlane = camSettings.outerFocus;
                CameraController.cameraControls.playerCamera.nearClipPlane = camSettings.innerFocus;
                var sliders = canvasInstance.GetComponentsInChildren<UnityEngine.UI.Slider>();
                speedometer = sliders[0];
                tachometer = sliders[1];
                lapTime = canvasInstance.GetComponentInChildren<UnityEngine.UI.Text>();
                tachometer.maxValue = CameraController.cameraControls.playerControls.engine.engTraits.tachometerLimit;
                speedometer.maxValue = CameraController.cameraControls.playerControls.target.Traits.topSpeed;
            }
            else
            {
                Debug.Log("Null on hud creation.");
            }
        }

        public void UpdateTachometer()
        {
            tachometer.value = Mathf.Abs(CameraController.cameraControls.playerControls.engine.speed);
        }
        public void UpdateSpeedometer()
        {
            speedometer.value = Mathf.Abs(CameraController.cameraControls.playerControls.target.Velocity.magnitude);
        }
        public void Update()
        {
            if (CameraController.cameraControls.playerControls.target != null)
            {
                UpdateSpeedometer();
                UpdateTachometer();
                lapTimeCounter += Time.deltaTime;
                lapTime.text = string.Format("Time: {0}", lapTimeCounter);
            }
        }

    }
}