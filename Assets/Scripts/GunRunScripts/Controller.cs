
using UnityEngine;


namespace com.braineeeeDevs.gr
{
    /// <summary>
    /// The tie-in controller for controlling GroundVehicles.
    /// </summary>
    public class Controller : MonoBehaviour
    {
        public GroundVehicle puppet;
        public Vector3 mousePosition, drive_and_steering;
        public float roll = 0f;
        protected bool applyBrakes, shiftBackward = false, toggleParkState = false, shiftForward = false, toggleLights = false, toggleFogLights = false;
        /// <summary>
        /// Sets the vehicle under control.
        /// </summary>
        /// <param name="p">The vehicle to take control of.</param>
        public void SetPuppet(GroundVehicle p)
        {
            this.puppet = p;
        }
        /// <summary>
        /// Retrieves inputs from the Unity InputManager. (The new InputSystem sucks).
        /// </summary>
        public void GetInputs()
        {
            applyBrakes = Input.GetAxisRaw("Jump") > 0f;
            toggleLights = Input.GetAxisRaw("Headlights") > 0f;
            toggleFogLights = Input.GetAxisRaw("Foglights") > 0f;
            toggleParkState = Input.GetAxisRaw("ParkShifter") > 0f;
            shiftForward = Input.GetAxis("Vertical") > 0f;
            shiftBackward = Input.GetAxis("Vertical") < 0f;
            roll = Input.GetAxis("Roll");
            drive_and_steering = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
            mousePosition = new Vector3(Input.mousePosition.x - Screen.width * 0.5f, Input.mousePosition.y - Screen.height * 0.5f, Input.GetAxisRaw("Mouse ScrollWheel"));
        }
        /// <summary>
        /// Applies the input to the vehicle under control.
        /// </summary>
        void ApplyInputsTo()
        {
            puppet.applyingBrakes = applyBrakes;
            puppet.SteeringAndDrive = drive_and_steering;
            if (toggleParkState)
            {
                puppet.ShiftPark();
            }
            if (toggleLights)
            {
                puppet.ToggleHeadLamps();
            }
            if (toggleFogLights)
            {
                puppet.ToggleFogLamps();
            }
        }

        private void Update()
        {
            GetInputs();
        }


        void FixedUpdate()
        {
            if (puppet != null)
            {
                ApplyInputsTo();
            }
        }

    }
}
