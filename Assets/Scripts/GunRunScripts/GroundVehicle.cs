using UnityEngine;

namespace com.braineeeeDevs.gunRun
{
    /// <summary>
    /// A four wheel vehicle to drive.
    /// </summary>
    [RequireComponent(typeof(Calculus))]
    public class GroundVehicle : BasicObject, IPublish, ITakeDamage
    {
        public bool isPlayer = false;
        protected uint lampLevel = 0, prevLampLevel = 0;
        protected float damagePoints;
        public Wheel[] wheels = new Wheel[MathUtilities.wheelQuantity];
        public Transform orbiter;
        public float rightingForce = 3f;
        public bool applyingBrakes = true, TCEngaged = false, allWheelsGrounded = false, allWheelsStopped = false;
        protected Vector2 driveInput;
        public Calculus vehicleCalculus;
        public Engine engine;
        public Transmission transmission;
        public Differential front_differential, rear_differential;
        public SwayBar front_swaybar, rear_swaybar;
        public Light[] headlight, foglight;
        /// <summary>
        /// The physics simulator component.
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
        /// <value>X component is used in steering while the Z component is used in drive direction.</value>
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

        public Vector3 Velocity
        {
            get
            {
                return (vehicleCalculus.ThreeSpaceVelocity);
            }
        }
        public Vector3 Acceleration
        {
            get
            {
                return (vehicleCalculus.ThreeSpaceAcceleration);
            }
        }
        public void Awake()
        {
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

        void FixedUpdate()
        {
            vehicleCalculus.Compute(transform.position);
            engine.Operate(driveInput.y);
            transmission.Operate(engine.speed);

            front_differential.Operate(transmission.outputTorque * 0.5f);
            rear_differential.Operate(transmission.outputTorque * 0.5f);

            front_swaybar.Operate();
            rear_swaybar.Operate();

            float radius = wheels[0].wheelCollider.radius;
            wheels[0].SteerAngle = SteeringAndDrive.x > 0f ? ComputeAckermannSteering(radius, false) : ComputeAckermannSteering(radius, true);
            wheels[1].SteerAngle = SteeringAndDrive.x > 0f ? ComputeAckermannSteering(radius, false) : ComputeAckermannSteering(radius, true);


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
        public void ToggleHeadLamps()
        {
            headlight[0].enabled = headlight[1].enabled = !(headlight[0].enabled && headlight[1].enabled);
            foglight[0].enabled = foglight[1].enabled = false;

        }
        public void ToggleHighBeams()
        {
            headlight[0].enabled = headlight[1].enabled = true;
            foglight[0].enabled = foglight[1].enabled = !(foglight[0].enabled && foglight[1].enabled);
        }
        public void Shift()
        {
            if (allWheelsStopped)
                TCEngaged = transmission.torqueConverterEngaged = !transmission.torqueConverterEngaged;
        }

        protected float ComputeAckermannSteering(float r, bool isInner)
        {
            var val = 0f;
            if (isInner)
            {
                val = Mathf.Atan(traits.wheelBaseLength * SteeringAndDrive.x / (traits.turnRadius - (traits.wheelBaseWidth * 0.5f)));
            }
            val = Mathf.Atan(traits.wheelBaseLength * SteeringAndDrive.x / (traits.turnRadius + (traits.wheelBaseWidth * 0.5f)));
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

        void ITakeDamage.ApplyDamage(float quantity)
        {

        }
        void ITakeDamage.Hit()
        {

        }
        void ITakeDamage.Die()
        {

        }

    }

}
