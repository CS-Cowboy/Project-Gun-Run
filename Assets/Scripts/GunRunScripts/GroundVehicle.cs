using UnityEngine;

namespace com.braineeeeDevs.gr
{
    /// <summary>
    /// A four wheel vehicle to drive.
    /// </summary>
    [RequireComponent(typeof(Calculus))]
    public class GroundVehicle : BasicObject, IPublish, IShowDamage 
    {
        public bool isPlayer = false;
        public VehicleTraits vehicleTraits;
        protected uint lampLevel = 0, prevLampLevel = 0;
        protected float damagePoints, whlVel;
        public Wheel[] wheels = new Wheel[MathUtilities.wheelQuantity];
        public Transform orbiter;
        public float rightingForce = 7f;
        public bool applyingBrakes = true, TCEngaged = false, allWheelsGrounded = false, allWheelsStopped = false;
        protected Vector2 driveInput;
        public Calculus vehicleCalculus;
        public Engine engine;
        public Transmission transmission;
        public Differential front_differential, rear_differential;
        public SwayBar front_swaybar, rear_swaybar;
        public Light[] headlight, foglight;
        /// <summary>
        /// The physics simulating component.
        /// </summary>
        /// <value>A rigidbody belonging to the vehicle gameObject.</value>
        public Rigidbody RBPhysics
        {
            get
            {
                return this.rbPhysics;
            }
        }
        /// <summary>
        /// The steering (x) and drive (z) inputs.
        /// </summary>
        /// <value>X component is used in steering while the Y component is used in drive direction.</value>
        public Vector2 SteeringAndDrive
        {
            set
            {
                driveInput = value;
            }
            get
            {
                return driveInput;
            }
        }
        /// <summary>
        /// The velocity of the vehicle in world space.
        /// </summary>
        /// <value>A Vector3 containing the vehicle's Velocity.</value>        
        public Vector3 Velocity
        {
            get
            {
                return (vehicleCalculus.ThreeSpaceVelocity);
            }

        }
        /// <summary>
        /// The Acceleration of the vehicle in world space.
        /// </summary>
        /// <value>A Vector3 containing the vehicle's Acceleration.</value>        
        public Vector3 Acceleration
        {
            get
            {
                return (vehicleCalculus.ThreeSpaceAcceleration);
            }
        }

        public float WheelVelocity
        {
            get
            {
                Debug.Log(string.Format("Wheel Velocity ->{0} ", whlVel));
                return whlVel;
            }
        }
        public override void Awake()
        {
            base.Awake();
            vehicleCalculus = GetComponent<Calculus>();
        }
        public override void Start()
        {
            base.Start();
            damagePoints = 0f;
            if (isPlayer)
            {
                CameraController.AttachTo(this);
            }
            TCEngaged = transmission.torqueConverterEngaged;
        }
        /// <summary>
        /// Drives all the vehicle's physical operations in the Physics timestep.
        /// </summary>
        void FixedUpdate()
        {
            vehicleCalculus.Compute(transform.position);
            engine.Operate(SteeringAndDrive.y);
            transmission.Operate(engine.speed);

            front_differential.Operate(transmission.outputTorque * 0.5f);
            rear_differential.Operate(transmission.outputTorque * 0.5f);

            front_swaybar.Operate();
            rear_swaybar.Operate();

            float radius = wheels[0].wheelCollider.radius;
            wheels[0].SteerAngle = SteeringAndDrive.x > 0f ? ComputeAckermannSteering(radius, false) : ComputeAckermannSteering(radius, true);
            wheels[1].SteerAngle = SteeringAndDrive.x > 0f ? ComputeAckermannSteering(radius, false) : ComputeAckermannSteering(radius, true);

            whlVel = Mathf.Abs(0.25f * (wheels[0].AngularVelocity + wheels[1].AngularVelocity + wheels[2].AngularVelocity + wheels[3].AngularVelocity));

            if (applyingBrakes)
            {
                wheels[0].Operate(0f);
                wheels[1].Operate(0f);

                wheels[2].Operate(0f);
                wheels[3].Operate(0f);

                wheels[0].ApplyBrake();
                wheels[1].ApplyBrake();

                wheels[2].ApplyBrake();
                wheels[3].ApplyBrake();
            }
            else
            {
                wheels[0].Operate(front_differential.left_torque);
                wheels[1].Operate(front_differential.right_torque);

                wheels[2].Operate(rear_differential.left_torque);
                wheels[3].Operate(rear_differential.right_torque);
            }

            allWheelsGrounded = wheels[0].wheelCollider.isGrounded && wheels[2].wheelCollider.isGrounded || wheels[1].wheelCollider.isGrounded && wheels[3].wheelCollider.isGrounded;
            allWheelsStopped = !(wheels[0].isTurning && wheels[2].isTurning && wheels[1].isTurning && wheels[3].isTurning);

            if (!allWheelsGrounded)
            {
                ApplyRightingForce();
            }
        }
        /// <summary>
        /// Toggles the headlights on or off.
        /// </summary>
        public void ToggleHeadLamps()
        {
            headlight[0].enabled = headlight[1].enabled = !(headlight[0].enabled && headlight[1].enabled);
            foglight[0].enabled = foglight[1].enabled = false;
            ToggleHighBeams(headlight[0].enabled && headlight[1].enabled);
        }

        public void ToggleHighBeams(bool usingHeadLamps)
        {
            if (usingHeadLamps)
            {
                headlight[0].range = headlight[1].range = vehicleTraits.highBeamRange;
                headlight[0].spotAngle = headlight[1].spotAngle = vehicleTraits.highBeamRange;
            }
            else
            {
                headlight[0].range = headlight[1].range = vehicleTraits.headlampRange;
                headlight[0].spotAngle = headlight[1].spotAngle = vehicleTraits.headlampSpotAngle;
            }
        }
        /// <summary>
        /// Toggles the headlights and the fog lights on or off.
        /// </summary>
        public void ToggleFogLamps()
        {
            headlight[0].enabled = headlight[1].enabled = true;
            foglight[0].enabled = foglight[1].enabled = !(foglight[0].enabled && foglight[1].enabled);
        }
        public void ShiftPark()
        {
            if (allWheelsStopped)
                TCEngaged = transmission.torqueConverterEngaged = !transmission.torqueConverterEngaged;
        }

        protected float ComputeAckermannSteering(float r, bool isInner)
        {
            var val = 0f;
            if (isInner)
            {
                val = Mathf.Atan(vehicleTraits.wheelBaseLength * SteeringAndDrive.x / (vehicleTraits.turnRadius - (vehicleTraits.wheelBaseWidth * 0.5f)));
            }
            val = Mathf.Atan(vehicleTraits.wheelBaseLength * SteeringAndDrive.x / (vehicleTraits.turnRadius + (vehicleTraits.wheelBaseWidth * 0.5f)));
            return val * Mathf.Rad2Deg;
        }
        /// <summary>
        /// Applies a torque along the vehicle's length to achieve the player's upright orientation.
        /// </summary>
        void ApplyRightingForce()
        {
            rbPhysics.AddRelativeTorque(Vector3.forward * driveInput.x * rbPhysics.mass * rightingForce);
        }
        void FireWeapon(Weapon target)
        {

        }

        void AimWeapon(Weapon target, Vector3 direction)
        {

        }
        void IPublish.Transmit()
        {

        }
        void IShowDamage.Apply(Vector3 direction)
        {
            
        }
    }

}
