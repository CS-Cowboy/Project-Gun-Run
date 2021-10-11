using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.braineeeeDevs.gr
{

    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(AudioSource))]

    public class Body : VehicleComponent
    {
        public bool isPlayer = false;
        protected Transform orbiter;
        protected Rigidbody simulator;
        protected AudioSource sfx;
        public Transform Orbiter
        {
            get
            {
                return orbiter;
            }
            set
            {
                orbiter = value;
            }
        }
        public Vector3 Velocity
        {
            get
            {
                return simulator.velocity;
            }
        }
        public void ApplyExtraForceAt(Vector3 localPosition, Vector3 relativeDirection)
        {
            simulator.AddForceAtPosition(relativeDirection, localPosition);
        }
        public override void Start()
        {
            owner = CameraController.cameraControls.playerControls;
            owner.AssignTarget(this);
            foreach (Transform t in GetComponentsInChildren<Transform>())
            {
                if (t.name.ToLower().Contains("orbiter"))
                {
                    orbiter = t;
                    Debug.Log("Orbiter found.");
                    break;
                }
            }
            sfx = GetComponent<AudioSource>();
            simulator = GetComponent<Rigidbody>();
            simulator.mass = componentTraits.mass;
            simulator.drag = simulator.angularDrag = componentTraits.drag;
            CameraController.cameraControls.playerCamera.transform.localPosition = CameraController.cameraControls.playerControls.target.Traits.cameraTraits.localPositionAtStart;
            CameraController.cameraControls.playerCamera.transform.rotation = CameraController.cameraControls.playerControls.target.transform.rotation;
        }
    }
}