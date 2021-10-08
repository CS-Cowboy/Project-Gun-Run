
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace com.braineeeeDevs.gr
{
    /// <summary>
    /// A class for importing vehicle models to functioning gameObject status; using the rules given in the "ReadMeToImportModels" guide in this project.
    /// The resulting GameObject is dependent upon the ObjectAttributes ScriptableObject provided by the user.
    /// </summary>
    public class VehicleWizard : ScriptableWizard
    {
        public string vehicleFilename = "Enter Value";
        private uint wheelCount = 0;
        public bool isPlayer = true;
        private bool isComplete = false;
        protected Wheel latestTire;
        private GameObject newVehicle;
        private GroundVehicle vehicle;
        public VehicleTraits attributes;
        private Wheel firstTireMade, lastTireMade;
        private Transform colliderChassis;
        static string wheelTag = "Wheel", colliderTag = "Collider";
        /// <summary>
        /// Used by the UnityEditor to      play a menu to use this class.
        /// </summary>
        [MenuItem("GunRun/Import Vehicle")]
        public static void CreateWizard()
        {
            ScriptableWizard.DisplayWizard<VehicleWizard>("Import Vehicle Model", "Create");
        }
        /// <summary>
        /// Builds the GroundVehicle gameObject from the imported model given in the "vehicleFileName" and a non-null "attributes" reference.
        /// </summary>
        protected void OnWizardCreate()
        {
            if (vehicleFilename != "Enter Value" && attributes != null)
            {
                GameObject obj = Resources.Load<GameObject>(vehicleFilename);
                GameObject child = obj.transform.GetChild(0).gameObject; //Get first child of file.
                newVehicle = Instantiate(child);
                this.RigVehicle();
                this.isComplete = true;
            }
        }
        /// <summary>
        /// Destroys the generated gameObject if the import process fails.
        /// </summary>
        void OnDestroy()
        {
            if (!isComplete)
            {
                DestroyImmediate(newVehicle);
            }
        }
        /// <summary>
        /// Creates the appropriate components and attaches them to the new GroundVehicle gameObject.
        /// </summary>
        protected void RigVehicle()
        {
            vehicle = newVehicle.AddComponent<GroundVehicle>();
            vehicle.vehicleTraits = attributes;

            if (this.BuildPlayer())
            {
                colliderChassis = new GameObject("Collider_Chassis").transform;
                colliderChassis.transform.SetParent(newVehicle.transform);
                colliderChassis.transform.localPosition = Vector3.zero;
                var subModels = newVehicle.GetComponentsInChildren<MeshRenderer>();
                List<MeshRenderer> colliderMeshes = new List<MeshRenderer>();
                List<MeshRenderer> wheelMeshes = new List<MeshRenderer>();
                List<MeshRenderer> componentMeshes = new List<MeshRenderer>();
                foreach (MeshRenderer mesh in subModels)
                {
                    string meshName = mesh.gameObject.name.ToLower();
                    if (meshName.Contains("collider"))
                    {
                        colliderMeshes.Add(mesh);
                    }
                    else if (meshName.Contains("wheel"))
                    {
                        wheelMeshes.Add(mesh);
                    }
                    else if (meshName.Contains("component"))
                    {
                        componentMeshes.Add(mesh);
                    }
                }

                vehicle.foglight = new Light[2];
                vehicle.headlight = new Light[2];
                foreach (Transform t in newVehicle.GetComponentsInChildren<Transform>())
                {
                    string transformName = t.name.ToLower();
                    if (transformName.Contains("component"))
                    {
                        if (transformName.Contains("headlamp"))
                        {
                            if (vehicle.headlight[0] == null)
                            {
                                vehicle.headlight[0] = BuildLight(t.gameObject, false);
                            }
                            else if (vehicle.headlight[1] == null)
                            {
                                vehicle.headlight[0] = BuildLight(t.gameObject, false);
                            }

                        }
                        else if (transformName.Contains("foglamp"))
                        {
                            if (vehicle.foglight[0] == null)
                            {
                                vehicle.foglight[0] = BuildLight(t.gameObject, true);
                            }
                            else if (vehicle.foglight[1] == null)
                            {
                                vehicle.foglight[1] = BuildLight(t.gameObject, true);
                            }
                        }
                    }
                }
                if (BuildWheels(wheelMeshes))
                {
                    BuildComponents(componentMeshes);
                    BuildColliders(colliderMeshes);
                    Vector3 wheelBaseDiagonal = firstTireMade.transform.position - lastTireMade.transform.position;
                    attributes.wheelBaseLength = Vector3.ProjectOnPlane(wheelBaseDiagonal, vehicle.transform.right).magnitude;
                    attributes.wheelBaseWidth = Vector3.ProjectOnPlane(wheelBaseDiagonal, vehicle.transform.forward).magnitude;
                    attributes.topSpeed = MathUtilities.MetricTopSpeedWithDrag(attributes.engineSpeedToTorqueGearCurve.Evaluate(0f) * attributes.finalDrive / MathUtilities.wheelQuantity, attributes.drag);

                }
            }
        }
        /// <summary>
        /// Builds the player if isPlayer is checked.
        /// </summary>
        /// <returns>A bool representing success (with true) or failure (with false).</returns>
        protected bool BuildPlayer()
        {
            var camera = newVehicle.GetComponentInChildren<Camera>();

            if (isPlayer)
            {
                camera.usePhysicalProperties = false;
                camera.fieldOfView = 60f;
                camera.nearClipPlane = 0f;
                camera.farClipPlane = 10000f;
                var controller = camera.gameObject.AddComponent<Controller>();
                var cameraControls = camera.gameObject.AddComponent<CameraController>();
                vehicle.orbiter = camera.transform.parent;
                controller.puppet = vehicle;
                vehicle.orbiter.localRotation = camera.transform.localRotation = Quaternion.identity;
                camera.transform.localPosition = new Vector3(0f, 2f, -6f);
                if (vehicle.vehicleTraits.hudPrefab != null)
                {
                    var hudInstance = Instantiate(vehicle.vehicleTraits.hudPrefab);
                    hudInstance.transform.SetParent(camera.transform);
                }
                else
                {
                    Debug.LogError("Model import failed, because the OjbectAttributes supplied has not set the 'HudPrefab' variable.");
                    isComplete = false;
                    return false;
                }
            }
            else
            {
                DestroyImmediate(camera);
            }
            vehicle.isPlayer = isPlayer;
            return true;
        }
        /// <summary>
        /// Builds the wheels.
        /// </summary>
        /// <param name="meshes">The meshes which are to represent the wheels.</param>
        /// <returns>A bool representing success (with true) or failure (with false).</returns>
        protected bool BuildWheels(List<MeshRenderer> meshes)
        {
            vehicle.wheels = new Wheel[MathUtilities.wheelQuantity];
            Wheel leftWhl = null, rightWhl = null;
            if (meshes.Count == MathUtilities.wheelQuantity && meshes.Count % 2 == 0)
            {
                for (int c = 0; c < meshes.Count; c += 2)
                {
                    leftWhl = BuildWheel(meshes[c]);
                    rightWhl = BuildWheel(meshes[c + 1]);
                    if (c == 0)
                    {
                        firstTireMade = leftWhl;
                    }
                    vehicle.wheels[c] = leftWhl;
                    vehicle.wheels[c + 1] = rightWhl;
                }
                lastTireMade = rightWhl;

            }
            else
            {
                Debug.LogError("Model import failed. The quantity of wheel objects on your model differs from the 'wheelQuantity' variable set in MathUtilities.");
                isComplete = false;
                return false;
            }
            return true;
        }

        public void BuildComponents(List<MeshRenderer> meshes)
        {
            string meshName = "";
            foreach (MeshRenderer mesh in meshes)
            {
                meshName = mesh.name.ToLower();
                if (meshName.Contains("differential"))
                {
                    if (vehicle.front_differential == null)
                    {
                        vehicle.front_differential = mesh.gameObject.AddComponent<Differential>();
                        vehicle.front_differential.owner = vehicle;
                    }
                    else
                    {
                        vehicle.rear_differential = mesh.gameObject.AddComponent<Differential>();
                        vehicle.rear_differential.owner = vehicle;
                    }

                }
                else if (meshName.Contains("engine"))
                {
                    vehicle.engine = mesh.gameObject.AddComponent<Engine>();
                    vehicle.engine.owner = vehicle;
                }
                else if (meshName.Contains("transmission"))
                {
                    vehicle.transmission = mesh.gameObject.AddComponent<Transmission>();
                    vehicle.transmission.owner = vehicle;
                }
                else if (meshName.Contains("swaybar"))
                {
                    if (vehicle.front_swaybar == null)
                    {
                        vehicle.front_swaybar = mesh.gameObject.AddComponent<SwayBar>();
                        vehicle.front_swaybar.owner = vehicle;
                        vehicle.front_swaybar.left = vehicle.wheels[0];
                        vehicle.front_swaybar.right = vehicle.wheels[1];
                    }
                    else
                    {
                        vehicle.rear_swaybar = mesh.gameObject.AddComponent<SwayBar>();
                        vehicle.rear_swaybar.owner = vehicle;
                        vehicle.rear_swaybar.left = vehicle.wheels[2];
                        vehicle.rear_swaybar.right = vehicle.wheels[3];
                    }
                }
            }
        }
        protected Light BuildLight(GameObject obj, bool isFogLamp)
        {
            var lamp = obj.AddComponent<Light>();
            if (!isFogLamp)
            {
                lamp.range = vehicle.vehicleTraits.headlampRange;
                lamp.spotAngle = vehicle.vehicleTraits.headlampAngle;
            }
            else
            {
                lamp.range = vehicle.vehicleTraits.foglampRange;
                lamp.spotAngle = vehicle.vehicleTraits.foglampAngle;
            }
            lamp.lightmapBakeType = LightmapBakeType.Mixed;
            lamp.renderMode = LightRenderMode.Auto;
            return lamp;
        }
        /// <summary>
        /// Builds an individual wheel.
        /// </summary>
        /// <param name="mesh">One mesh to represent the wheel.</param>
        /// <returns>A bool representing success (with true) or failure (with false).</returns>
        protected Wheel BuildWheel(MeshRenderer mesh)
        {
            var newWheel = mesh.gameObject.AddComponent<Wheel>();
            var colliderObject = new GameObject("[Collider]");
            newWheel.owner = vehicle;
            newWheel.mesh = mesh;
            colliderObject.name = mesh.gameObject.name + colliderObject.name;
            newWheel.wheelCollider = colliderObject.AddComponent<WheelCollider>();
            newWheel.wheelCollider.radius = GetTireSize(mesh, newWheel);
            WheelFrictionCurve friction = new WheelFrictionCurve();
            friction.extremumSlip = 0.4f;
            friction.extremumValue = 1f;
            friction.asymptoteSlip = 0.8f;
            friction.asymptoteValue = 0.5f;
            friction.stiffness = vehicle.vehicleTraits.forwardFrictionStiffness;
            newWheel.wheelCollider.suspensionDistance = vehicle.vehicleTraits.suspensionDistance;
            newWheel.wheelCollider.forwardFriction = friction;

            friction.stiffness = vehicle.vehicleTraits.sideFrictionStiffness;
            newWheel.wheelCollider.sidewaysFriction = friction;
            //Set tags
            newWheel.wheelCollider.tag = colliderTag;
            mesh.tag = wheelTag;
            //Attach collider to axle.
            float sign = mesh.transform.localPosition.x < 0f ? -1f : +1f;
            colliderObject.transform.SetParent(mesh.transform.parent.parent);
            colliderObject.transform.position = mesh.transform.parent.position;
            colliderObject.transform.rotation = mesh.transform.parent.rotation;

            if (wheelCount < MathUtilities.wheelQuantity)
            {
                wheelCount++;
                newWheel.wheelNumber = wheelCount;
            }

            return newWheel;
        }
        /// <summary>
        /// Finds the tire size based on the wheel meshes geometry. Makes the assumption that the wheel is wider in diameter than it is thick.
        /// </summary>
        /// <param name="mesh">The wheel mesh</param>
        /// <param name="target">The Wheel MonoBehavior component it belongs to.</param>
        /// <returns></returns>
        protected float GetTireSize(MeshRenderer mesh, Wheel target)
        {
            Vector3 tireDimensions = mesh.bounds.size;
            float x, y, z;
            x = tireDimensions.x;
            y = tireDimensions.y;
            z = tireDimensions.z;
            target.tireDiameterInMeters = Mathf.Max(x, y, z);
            return target.tireDiameterInMeters * 0.5f;
        }
        /// <summary>
        /// Builds the collider objects for the GroundVehicle. These colliders are used in determing center of mass for the vehicle.
        /// </summary>
        /// <param name="meshes">The meshes which are to represent the colliders, which will be invisible.</param>
        protected void BuildColliders(List<MeshRenderer> meshes)
        {
            if (meshes.Count > 0)
            {
                foreach (MeshRenderer mesh in meshes)
                {
                    mesh.gameObject.AddComponent<MeshCollider>().convex = true;
                    DestroyImmediate(mesh); //Force invisibility on mesh 
                }
            }
            else
            {
                Debug.LogWarning("Model did not contain any named collider meshes. Defaulting to a convex mesh collider.");
                newVehicle.AddComponent<MeshCollider>().convex = true;
            }

        }
    }
}