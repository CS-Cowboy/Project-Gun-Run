using UnityEngine;
using UnityEngine.UI;
namespace com.braineeeeDevs.gunRun
{
    public class PlayerHUD : MonoBehaviour
    {
        //Use cases: a. race position(S)
        //b. speed
        //c. acceleration/tachometer
        //d. race time
        //e. map!

        protected UnityEngine.UI.Slider speedometer, tachometer;
        protected UnityEngine.UI.Text lapTime;
        protected float lapTimeCounter;
        public Canvas canvasInstance;
        void Start()
        {
            this.BuildHUD();
        }
        /// <summary>
        /// Builds the Heads-Up-Display from the ObjectAttributes 'HudPrefab' variable and attaches it to the player's camera. 
        /// </summary>
        void BuildHUD()
        {
            if (CameraController.playerControls.puppet != null && CameraController.playerControls.puppet.traits != null)
            {
                var hudObj = Instantiate(CameraController.playerControls.puppet.traits.hudPrefab);
                canvasInstance = hudObj.GetComponent<Canvas>();
                canvasInstance.renderMode = RenderMode.ScreenSpaceCamera;
                canvasInstance.worldCamera = CameraController.playerCamera;
                speedometer = canvasInstance.GetComponentInChildren<UnityEngine.UI.Slider>();
                tachometer = canvasInstance.GetComponentInChildren<UnityEngine.UI.Slider>();
                lapTime = canvasInstance.GetComponentInChildren<UnityEngine.UI.Text>();
                this.transform.SetParent(CameraController.playerCamera.transform);
            }
        }

        public void UpdateTachometer()
        {
            tachometer.value = (float)CameraController.playerControls.puppet.engine.speed;
        }
        public void UpdateSpeedometer()
        {
            speedometer.value = (float)CameraController.playerControls.puppet.vehicleCalculus.Velocity.magnitude;
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