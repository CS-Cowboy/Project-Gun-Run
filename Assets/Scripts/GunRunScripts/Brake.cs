namespace com.braineeeeDevs.gr
{
    public class Brake : VehicleComponent
    {
        protected Wheel target;
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
            owner = CameraController.cameraControls.playerControls;
            target = GetComponentInChildren<Wheel>();
        }
        public void ApplyBrake()
        {
            target.Collider.motorTorque = 0f;
            target.Collider.brakeTorque = owner.target.Traits.brakeTorque * effectiveness;
        }
    }

}