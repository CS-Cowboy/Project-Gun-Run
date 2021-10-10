using UnityEngine;
using System.Collections;
namespace com.braineeeeDevs.gr
{
    [System.Serializable]
    /// <summary>
    /// A class to represent and tie together wheel collider, meshes, and functionality.
    /// </summary>
    [RequireComponent(typeof(Calculus))]
    public class Wheel : VehicleComponent
    {
        public WheelCollider wheelCollider;
        public MeshRenderer mesh;
        protected Calculus wheelCalculus;
        public float mTorq, bTorq, velocity, acceleration, slip, tireDiameterInMeters, inertia, drivingForce, antiRollForce = 1f;
        public uint wheelNumber = 0;
        public bool isTurning = false, isBraking = true, inDrive = false;
        ITakeDamage damage;
        public void Awake()
        {
            damage = GetComponent<ITakeDamage>();
        }
        /// <summary>
        /// Velocity of the wheel (rev/min)
        /// </summary>
        /// <value>float containing angular velocity.</value>
        public float AngularVelocity
        {
            get
            {
                return velocity;
            }
        }
        /// <summary>
        /// Acceleration of the wheel (rev/min^2)
        /// </summary>
        /// <value>float containing angular acceleration.</value>
        public float AngularAcceleration
        {
            get
            {
                return acceleration;
            }
        }
        /// <summary>
        /// The force exerted on vehicle from the anti-roll bars (Nm)
        /// </summary>
        /// <value>The anti-roll force (Nm). </value>
        public float SwayBarForce
        {
            set
            {
                antiRollForce = value > 0f ? value : -1f * value;
            }
        }
        /// <summary>
        /// The angle to steer with.
        /// </summary>
        /// <value>float in degrees to steer this wheel.</value>
        public float SteerAngle
        {
            set
            {
                wheelCollider.steerAngle = value;
            }
        }
        public override void Start()
        {
            base.Start();
            owner = GetComponentInParent<GroundVehicle>();
            mesh = GetComponent<MeshRenderer>();
            wheelCalculus = GetComponent<Calculus>();
        }
        /// <summary>
        /// Drives the given wheel. Divides the given engine torque to individual wheel torque range.
        /// </summary>
        /// <param name="inputTorque">The engine torque (Nm).</param>
        /// <param name="steerAngle">The angle to steer at (degrees) </param>
        public override void Operate(float inputTorque)
        {
            if (owner != null && wheelCollider != null)
            {
                inputTorque *= damage.EvaluateHits();
                mTorq = wheelCollider.motorTorque = inputTorque;
                bTorq = wheelCollider.brakeTorque = 0f;
                ComputeSlip(inputTorque);
                ApplyPose();
                owner.RBPhysics.AddForceAtPosition(Vector3.up * -antiRollForce, transform.parent.position);
                isTurning = Mathf.Abs(AngularVelocity) >= 1f;
            }
        }
        public void ApplyBrake()
        {
            mTorq = wheelCollider.motorTorque = 0f;
            bTorq = wheelCollider.brakeTorque = ((owner.traits.mass + wheelCollider.mass) * owner.vehicleTraits.brakingForce);
        }
        /// Computes the wheel's slip and speed.    
        /// </summary>
        protected void ComputeSlip(float inputTorque)
        {
            velocity = owner.RBPhysics.velocity.magnitude;
            wheelCalculus.Compute(velocity);
            acceleration = wheelCalculus.Velocity;  //Because velocity is already computed Velocity is actually the acceleration in this case.
            var rOmega = Mathf.Clamp(AngularVelocity * wheelCollider.radius, wheelCollider.radius, owner.traits.topSpeed * wheelCollider.radius);
            inertia = Mathf.Clamp(AngularVelocity, 1f, owner.traits.topSpeed) * Mathf.Pow(wheelCollider.radius, 2f) * wheelCollider.mass;
            slip = (rOmega - owner.RBPhysics.velocity.magnitude) / rOmega;
            var a = (owner.Acceleration.magnitude / Mathf.Clamp(owner.Velocity.magnitude, 1f, float.MaxValue)) * (slip - 1f);
            var b = (owner.Velocity.magnitude / (inertia * wheelCollider.radius * Mathf.Clamp(Mathf.Pow(AngularVelocity, 2f), 1f, float.MaxValue))) * (inputTorque - wheelCollider.radius * (drivingForce));
            drivingForce = a + b;

        }

        /// <summary>
        /// Applies the wheel's position and rotation changes to the wheel mesh.
        /// </summary>
        protected void ApplyPose()
        {
            Vector3 wheelPos = new Vector3();
            Quaternion wheelRot = new Quaternion();
            wheelCollider.GetWorldPose(out wheelPos, out wheelRot);
            transform.parent.rotation = wheelRot * Quaternion.Euler(0f, 180f, 0f);
        }

    }
}