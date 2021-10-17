using UnityEngine;

namespace com.braineeeeDevs.gr
{
    public class Brake : VehicleComponent
    {
        [SerializeField] protected Wheel target;
        public Wheel Target
        {
            set { target = value; }
            get
            {
                return target;
            }
        }
        public override void Start()
        {
            target = GetComponentInChildren<Wheel>();
        }
        public void ApplyBrake()
        {
            if (target.Collider != null)
            {
                target.Collider.motorTorque = 0f;
                target.Collider.brakeTorque = owner.target.Traits.brakeTorque * effectiveness;
            }
        }
    }

}