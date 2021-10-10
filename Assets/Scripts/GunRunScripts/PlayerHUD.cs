using UnityEngine;
using UnityEngine.UI;
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
            if (CameraController.playerControls.puppet != null && CameraController.playerControls.puppet.vehicleTraits != null)
            {
                var hudObj = Instantiate(CameraController.playerControls.puppet.vehicleTraits.hudPrefab);
                canvasInstance = hudObj.GetComponent<Canvas>();
                canvasInstance.renderMode = camSettings.renderMode;
                canvasInstance.worldCamera = CameraController.playerCamera;
                CameraController.playerCamera.fieldOfView = camSettings.cameraAngle;
                CameraController.playerCamera.usePhysicalProperties = false;
                CameraController.playerCamera.focalLength = camSettings.focusLength;
                CameraController.playerCamera.farClipPlane = camSettings.outerFocus;
                CameraController.playerCamera.nearClipPlane = camSettings.innerFocus;
                var sliders = canvasInstance.GetComponentsInChildren<UnityEngine.UI.Slider>();
                speedometer = sliders[0];
                tachometer = sliders[1];
                lapTime = canvasInstance.GetComponentInChildren<UnityEngine.UI.Text>();
                tachometer.maxValue = CameraController.playerControls.puppet.vehicleTraits.tachometerLimit;
                speedometer.maxValue = CameraController.playerControls.puppet.traits.topSpeed;
            }
            else
            {
                Debug.Log("Null on hud creation.");
            }
        }

        public void UpdateTachometer()
        {
            tachometer.value = Mathf.Abs( CameraController.playerControls.puppet.engine.speed);
        }
        public void UpdateSpeedometer()
        {
            speedometer.value = Mathf.Abs(CameraController.playerControls.puppet.WheelVelocity );
        }
        public void Update()
        {
            if (CameraController.playerControls.puppet != null)
            {
                UpdateSpeedometer();
                UpdateTachometer();
                lapTimeCounter += Time.deltaTime;
                lapTime.text = string.Format("Time: {0}", lapTimeCounter);
            }
        }

    }
}