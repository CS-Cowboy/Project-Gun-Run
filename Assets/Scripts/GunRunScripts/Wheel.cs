using UnityEngine;

namespace com.braineeeeDevs.gunRun
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
        public Calculus wheelCalculus;
        public float linearSlip, tireDiameterInMeters, inertia, nonLinearSlip, d = 1f, antiRollForce = 1f;
        public uint wheelNumber = 0;
        public float AngularVelocity
        {
            get{
                return  wheelCalculus.Velocity;
            }
        }
          public float AngularAcceleration
        {
            get{
                return  wheelCalculus.Acceleration;
            }
        }

        public float SwayBarForce
        {
            set
            {
                antiRollForce = value > 0f ? value : -1f * value;
            }
        }

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
        /// <param name="inputTorque">The engine torque.</param>
        /// <param name="steerAngle">The angle to steer at (if steerable).</param>
        public override void Operate(float inputTorque)
        {
            if (owner != null && wheelCollider != null)
            {
                owner.RBPhysics.AddForceAtPosition(Vector3.up * -antiRollForce, transform.parent.position);
                if (!owner.applyingBrakes)
                {
                    wheelCollider.brakeTorque = 0f;
                    wheelCollider.motorTorque = inputTorque;
                }
                else
                {
                    wheelCollider.brakeTorque = ((owner.traits.massInKg + wheelCollider.mass) * owner.traits.brakingForce);
                    wheelCollider.motorTorque = 0f;
                }
                ComputeSlip(inputTorque);
                ApplyPose();
            }
        }

        /* Saved for later reference**
         float antiRollForce = (transform.localPosition.x < 0f ? this.GetSwayBarForce() - other.GetSwayBarForce() : other.GetSwayBarForce() - this.GetSwayBarForce());
                        if (owner.applyingBrakes)
                        {
                            wheelCollider.brakeTorque = owner.traits.topSpeed * sim.mass / MathUtilities.wheelQuantity;
                        }
                        else
                        {
                            wheelCollider.brakeTorque = 0f;
                            if (isMotorized)
                            {
                                wheelCollider.motorTorque = inputTorque;
                            }
                        }
                        if (isSteerable)
                        {
                            this.SetSteerAngle(steerAngle);
                        }

         */
        /// <summary>
        /// Computes the wheel's slip and speed.
        /// </summary>
        protected float ComputeSlip(float inputTorque)
        {
            var rOmega = AngularVelocity * wheelCollider.radius;
            wheelCalculus.Compute(wheelCollider.transform.eulerAngles * Mathf.Deg2Rad); //Wrong units???
            inertia = AngularVelocity * Mathf.Pow(wheelCollider.radius, 2f) * wheelCollider.mass;
            linearSlip = (rOmega - owner.RBPhysics.velocity.magnitude) / rOmega;
            var term_1 = (owner.AngularAcceleration.magnitude / owner.AngularVelocity.magnitude) * (linearSlip - 1f);
            var term_2 = (owner.AngularVelocity.magnitude / inertia * wheelCollider.radius * Mathf.Pow(AngularVelocity, 2f) * (inputTorque - wheelCollider.radius * wheelCollider.sprungMass * AngularAcceleration));
            nonLinearSlip = term_1 + term_2 + d;
            //  Debug.Log(string.Format("NonLinearSlip for wheel {0}:=>> {1} ", wheelNumber, nonLinearSlip));
            return nonLinearSlip;
        }

        /// <summary>
        /// Applies the wheel's position and rotation changes to the wheel mesh.
        /// </summary>
        protected void ApplyPose()
        {
            Vector3 wheelPos = new Vector3();
            Quaternion wheelRot = new Quaternion();
            wheelCollider.GetWorldPose(out wheelPos, out wheelRot);
            transform.parent.position = wheelPos;
            transform.parent.rotation = wheelRot * Quaternion.Euler(0f, 180f, 0f);
        }

    }
}