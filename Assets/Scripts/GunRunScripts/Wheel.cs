using UnityEngine;
namespace com.braineeeeDevs.gr
{
    /// <summary>
    /// A class to represent and tie together wheel collider, meshes, and functionality.
    /// </summary>
    public class Wheel : VehicleComponent
    {
        [SerializeField] protected WheelCollider wheelCollider;
        public MeshRenderer mesh;
        public float tireDiameter, motorTorque, brakeTorque;
        public WheelCollider Collider
        {
            get
            {
                return wheelCollider;
            }
            set
            {
                if (wheelCollider != null)
                    wheelCollider = value;
            }
        }
        public override void Start()
        {
            GetComponent<MeshCollider>().transform.localScale = Vector3.one * 0.97f;
        }
        /// <summary>
        /// Drives the given wheel. Divides the given engine torque to individual wheel torque range.
        /// </summary>
        /// <param name="inputTorque">The engine torque (Nm).</param>
        /// <param name="steerAngle">The angle to steer at (degrees) </param>
        public override void Operate(float inputTorque)
        {
            base.Operate();
            if (effectiveness <= 0f && Collider != null)
            {
                Jetison();
            }
            else
            {
                if (wheelCollider != null)
                {
                    if (!owner.applyBrakes)
                    {
                        inputTorque *= effectiveness;
                        wheelCollider.motorTorque = inputTorque;
                        wheelCollider.brakeTorque = 0f;
                    }

                    motorTorque = wheelCollider.motorTorque;
                    brakeTorque = wheelCollider.brakeTorque;
                }
            } //ComputeSlip(inputTorque);
            ApplyPose();
        }
        ///<summary>
        /// Computes the wheel's slip and speed.    
        /// </summary>
 /*       protected void ComputeSlip(float inputTorque)
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
*/
        /// <summary>
        /// Applies the wheel's position and rotation changes to the wheel mesh.
        /// </summary>
        protected void ApplyPose()
        {
            Vector3 wheelPos = new Vector3();
            Quaternion wheelRot = new Quaternion();
            if (Collider != null)
            {
                wheelCollider.GetWorldPose(out wheelPos, out wheelRot);
                transform.parent.rotation = wheelRot * Quaternion.Euler(0f, 180f, 0f);
            }
        }
        protected void Jetison()
        {
            float whlSpd = wheelCollider.motorTorque;
            var mass = wheelCollider.mass;
            DestroyImmediate(wheelCollider);
            var rigidbdy = gameObject.AddComponent<Rigidbody>();
            this.transform.SetParent(null);
            rigidbdy.mass = mass;
            rigidbdy.AddRelativeTorque(Vector3.right * whlSpd);
        }
    }
}