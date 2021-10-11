
using UnityEngine;


namespace com.braineeeeDevs.gr
{
    /// <summary>
    /// The tie-in controller for controlling GroundVehicles.
    /// </summary>
    public class Controller : MonoBehaviour
    {
        public Body target;
        public Engine engine;
        public Transmission transmission;
        public SteeringMechanism steeringMechanism;
        public Vector3 mousePosition;
        public float steering, driving, roll = 0f;
        public bool applyBrakes, toggleLights = false, toggleFogLights = false, toggleParkState = false;

        protected struct Brakes
        {
            public Brake[] brks;
            public void Apply()
            {
                brks[0].ApplyBrake();
                brks[1].ApplyBrake();
                brks[2].ApplyBrake();
                brks[3].ApplyBrake();
            }
        }
        public struct Headlights
        {
            public bool state;
            public Light[] lights;
            public void ToggleState()
            {
                state = !state;
                lights[0].enabled = lights[1].enabled = state;
            }
        }

        protected Brakes brakes;
        protected Headlights lamps, foglamps;

        public void AssignTarget(Body obj)
        {
            target = obj;
            var wheels = obj.GetComponentsInChildren<Wheel>();
            var colliders =  obj.GetComponentsInChildren<WheelCollider>();
            wheels[0].Collider = colliders[0];
            wheels[1].Collider = colliders[1];
            wheels[2].Collider = colliders[2];
            wheels[3].Collider = colliders[3];
            engine = obj.GetComponentInChildren<Engine>();
            transmission = obj.GetComponentInChildren<Transmission>();
            brakes = new Brakes { brks = obj.GetComponentsInChildren<Brake>() };

            var lites = obj.GetComponentsInChildren<Light>();
            lamps = new Headlights { lights = new Light[2] { lites[0], lites[2] } };
            foglamps = new Headlights { lights = new Light[2] { lites[1], lites[3] } };
        }

        /// <summary>
        /// Retrieves inputs from the Unity InputManager. (The new InputSystem sucks).
        /// </summary>
        public void GetInputs()
        {
            applyBrakes = Input.GetAxisRaw("Jump") > 0f;
            toggleParkState = Input.GetAxisRaw("ParkShifter") > 0f;
            toggleLights = Input.GetAxisRaw("Headlights") > 0f;
            toggleFogLights = Input.GetAxisRaw("Foglights") > 0f;
            roll = Input.GetAxis("Roll");
            steering = Input.GetAxis("Horizontal");
            driving = Input.GetAxis("Vertical");
            mousePosition = new Vector3(Input.mousePosition.x - Screen.width * 0.5f, Input.mousePosition.y - Screen.height * 0.5f, Input.GetAxisRaw("Mouse ScrollWheel"));
        }
        /// <summary>
        /// Applies the input to the vehicle under control.
        /// </summary>
        void ApplyInputsTo()
        {
            engine.Operate(driving);
            transmission.SetDriveDirection(driving);
            steeringMechanism.Operate(steering);
            if (toggleParkState)
            {
                transmission.ShiftParkState();
            }
            if (applyBrakes)
            {
                brakes.Apply();
            }   
            if (toggleLights)
            {
                lamps.ToggleState();
            }
            if (toggleFogLights)
            {
                foglamps.ToggleState();
            }
        }

        private void Update()
        {
            GetInputs();
        }


        void FixedUpdate()
        {
            if (target != null)
            {
                ApplyInputsTo();
            }
        }

    }
}
