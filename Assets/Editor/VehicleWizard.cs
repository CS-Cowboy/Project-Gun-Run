
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace com.braineeeeDevs.gr.Editor
{
    /// <summary>
    /// A class for importing vehicle models to functioning gameObject status; using the rules given in the "ReadMeToImportModels" guide in this project.
    /// The resulting GameObject is dependent upon the ObjectAttributes ScriptableObject provided by the user.
    /// </summary>
    public class VehicleWizard : ScriptableWizard
    {
        public string vehicleFilename = "ford_f-150";
        public bool isPlayer = true;
        private bool isComplete = false;
        private Body vehicle;
        public ComponentTraits attributes;
        private Controller controller;
        private Wheel firstTire, lastTire;
        private SwayBar front_swaybar;
        private Wheel[] tmpWheels = new Wheel[UnitsHelper.wheelQuantity];
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
        /// Builds the VehicleComponent gameObject from the imported model given in the "vehicleFileName" and a non-null "attributes" reference.
        /// </summary>
        protected void OnWizardCreate()
        {
            if (vehicleFilename != "Enter Value" && attributes != null)
            {
                GameObject obj = Resources.Load<GameObject>(vehicleFilename);
                GameObject child = obj.transform.GetChild(0).gameObject; //Get first child of file.
                this.RigVehicle(Instantiate(child));
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
                DestroyImmediate(vehicle.gameObject);
            }
        }
        /// <summary>
        /// Creates the appropriate components and attaches them to the new VehicleComponent gameObject.
        /// </summary>
        protected void RigVehicle(GameObject newVehicle)
        {
            vehicle = newVehicle.gameObject.AddComponent<Body>();
            this.BuildPlayer();

            var colliderMeshes = this.GetSubMeshes("collider");
            var wheelMeshes = this.GetSubMeshes("wheel");
            var components = this.GetSubMeshes("component");
            var headlightTransforms = this.GetTransforms("headlamp", false);
            var foglightTransforms = this.GetTransforms("foglamp", true);

            if (BuildWheelsFrom(wheelMeshes))
            {
                BuldLightsFrom(headlightTransforms, foglightTransforms);
                BuildComponentsFrom(components);
                BuildCollidersFrom(colliderMeshes);
                Vector3 wheelBaseDiagonal = firstTire.transform.position - lastTire.transform.position;
                attributes.wheelBaseLength = Vector3.ProjectOnPlane(wheelBaseDiagonal, vehicle.transform.right).magnitude;
                attributes.wheelBaseWidth = Vector3.ProjectOnPlane(wheelBaseDiagonal, vehicle.transform.forward).magnitude;
            }
        }

        public List<MeshRenderer> GetSubMeshes(string lowercaseName)
        {
            var subMeshes = vehicle.gameObject.GetComponentsInChildren<MeshRenderer>();
            List<MeshRenderer> meshes = new List<MeshRenderer>();
            foreach (MeshRenderer mesh in subMeshes)
            {
                string meshName = mesh.gameObject.name.ToLower();
                if (meshName.Contains(lowercaseName))
                {
                    meshes.Add(mesh);
                }
            }
            return meshes;
        }

        public List<Transform> GetTransforms(string name, bool isFoglamp)
        {
            List<Transform> transforms = new List<Transform>();
            foreach (Transform t in vehicle.gameObject.GetComponentsInChildren<Transform>())
            {
                string transformName = t.name.ToLower();
                if (transformName.Contains("component") && transformName.Contains(name))
                {
                    transforms.Add(t);
                }
            }
            return transforms;
        }

        public void BuldLightsFrom(List<Transform> headlightTransforms, List<Transform> foglightTransforms)
        {
            int length = headlightTransforms.Count == foglightTransforms.Count ? headlightTransforms.Count : 0;
            for (int c = 0; c < length; c++)
            {
                Transform fog = foglightTransforms[c], head = headlightTransforms[c];
                BuildLight(fog.gameObject, true);
                BuildLight(head.gameObject, false);
            }
        }

        /// <summary>
        /// Builds the player if isPlayer is checked.
        /// </summary>
        /// <returns>A bool representing success (with true) or failure (with false).</returns>
        protected void BuildPlayer()
        {
            var camera = vehicle.gameObject.GetComponentInChildren<Camera>();
            vehicle.isPlayer = isPlayer;
            vehicle.GetComponent<MeshCollider>().enabled = false;
            vehicle.Traits = attributes;

            if (isPlayer)
            {
                camera.usePhysicalProperties = false;
                camera.fieldOfView = 60f;
                camera.nearClipPlane = 0f;
                camera.farClipPlane = 10000f;
                controller = camera.gameObject.AddComponent<Controller>();
                var cameraControls = camera.gameObject.AddComponent<CameraController>();
                cameraControls.settings = attributes.cameraTraits;
                vehicle.Orbiter = camera.transform.parent;
                controller.target = vehicle;

                vehicle.Owner = controller;
                vehicle.Orbiter.localRotation = camera.transform.localRotation = Quaternion.identity;
                camera.transform.localPosition = new Vector3(0f, 2f, -6f);
                if (vehicle.Traits.hudPrefab != null)
                {
                    var hudInstance = Instantiate(vehicle.Traits.hudPrefab);
                    hudInstance.transform.SetParent(camera.transform);
                }
            }
            else
            {
                DestroyImmediate(camera);
            }

        }
        /// <summary>
        /// Builds the wheels.
        /// </summary>
        /// <param name="meshes">The meshes which are to represent the wheels.</param>
        /// <returns>A bool representing success (with true) or failure (with false).</returns>
        protected bool BuildWheelsFrom(List<MeshRenderer> meshes)
        {
            Wheel whl = null;
            if (meshes.Count == UnitsHelper.wheelQuantity && meshes.Count % 2 == 0)
            {
                for (int c = 0; c < meshes.Count; c++)
                {
                    whl = BuildWheel(meshes[c]);
                    whl.Traits = attributes;
                    if (c == 0)
                    {
                        firstTire = whl;
                    }
                    tmpWheels[c] = whl;
                    Debug.LogWarning(string.Format("WheelCollider->{0} ", whl.Collider));
                    var brake = whl.transform.parent.gameObject.AddComponent<Brake>();
                    brake.Target = whl;
                    brake.Traits = attributes;
                    brake.Owner = controller;
                }
                lastTire = whl;

            }
            else
            {
                Debug.LogError("Model import failed. The quantity of wheel objects on your model differs from the 'wheelQuantity' variable set in MathUtilities.");
                isComplete = false;
                return false;
            }
            return true;
        }

        public void BuildComponentsFrom(List<MeshRenderer> meshes)
        {
            string meshName = "";
            foreach (MeshRenderer mesh in meshes)
            {
                meshName = mesh.name.ToLower();
                if (meshName.Contains("differential"))
                {
                    var diff = mesh.gameObject.AddComponent<Differential>();
                    if (meshName.Contains("front"))
                    {
                        diff.left = tmpWheels[0];
                        diff.right = tmpWheels[1];
                    }
                    else
                    {
                        diff.left = tmpWheels[2];
                        diff.right = tmpWheels[3];
                    }
                }
                else if (meshName.Contains("engine"))
                {
                    controller.engine = mesh.gameObject.AddComponent<Engine>();
                    controller.engine.engTraits = attributes.engineTraits;
                }
                else if (meshName.Contains("transmission"))
                {
                    controller.transmission = mesh.gameObject.AddComponent<Transmission>();
                }
                else if (meshName.Contains("swaybar"))
                {
                    if (front_swaybar == null)
                    {
                        front_swaybar = mesh.gameObject.AddComponent<SwayBar>();

                        front_swaybar.Wheels = new Wheel[2] { tmpWheels[0], tmpWheels[1] };
                    }
                    else
                    {
                        var swaybar = mesh.gameObject.AddComponent<SwayBar>();

                        swaybar.Wheels = new Wheel[2] { tmpWheels[2], tmpWheels[3] };
                    }
                }
                else if (meshName.Contains("steering"))
                {
                    var steering = mesh.gameObject.AddComponent<SteeringMechanism>();
                    steering.Wheels = new Wheel[2] { tmpWheels[0], tmpWheels[1] };
                    controller.steeringMechanism = steering;
                }
                var meshCollider = mesh.GetComponent<MeshCollider>();
                meshCollider.convex = true;
                meshCollider.transform.localScale = Vector3.one * 0.97f;
                var comp = mesh.GetComponent<VehicleComponent>();
                comp.Owner = controller;
                comp.Traits = attributes;
            }
        }
        protected Light BuildLight(GameObject obj, bool isFogLamp)
        {
            var lamp = obj.AddComponent<Light>();
            lamp.range = !isFogLamp ? vehicle.Traits.headlampRange : vehicle.Traits.foglampRange;
            lamp.spotAngle = !isFogLamp ? vehicle.Traits.headlampSpotAngle : vehicle.Traits.foglampAngle;
            lamp.lightmapBakeType = LightmapBakeType.Mixed;
            lamp.type = LightType.Spot;
            lamp.renderMode = LightRenderMode.Auto;
            lamp.transform.rotation = Quaternion.LookRotation(vehicle.transform.forward, Vector3.up) * (isFogLamp ? Quaternion.Euler(attributes.foglampAxisAngle, 0f, 0f) : Quaternion.Euler(attributes.headlampAxisAngle, 0f, 0f));
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
            newWheel.mesh = mesh;
            var collider = newWheel.gameObject.GetComponent<MeshCollider>();
            collider.convex = true;
            collider.transform.localScale = Vector3.one * 0.97f;
            colliderObject.name = mesh.gameObject.name + colliderObject.name;
            newWheel.Collider = colliderObject.AddComponent<WheelCollider>();
            newWheel.Owner = controller;
            newWheel.Collider.radius = GetTireSize(mesh, newWheel);
            WheelFrictionCurve friction = new WheelFrictionCurve();
            friction.extremumSlip = 0.4f;
            friction.extremumValue = 1f;
            friction.asymptoteSlip = 0.8f;
            friction.asymptoteValue = 0.5f;
            friction.stiffness = vehicle.Traits.forwardFrictionStiffness;
            newWheel.Collider.suspensionDistance = vehicle.Traits.suspensionDistance;
            newWheel.Collider.forwardFriction = friction;

            friction.stiffness = vehicle.Traits.sideFrictionStiffness;
            newWheel.Collider.sidewaysFriction = friction;
            //Set tags
            newWheel.Collider.tag = colliderTag;
            mesh.tag = wheelTag;
            //Attach collider to axle.
            float sign = mesh.transform.localPosition.x < 0f ? -1f : +1f;
            colliderObject.transform.SetParent(mesh.transform.parent.parent);
            colliderObject.transform.position = mesh.transform.parent.position;
            colliderObject.transform.rotation = mesh.transform.parent.rotation;

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
            target.tireDiameter = Mathf.Max(x, y, z);
            return target.tireDiameter * 0.5f;
        }
        /// <summary>
        /// Builds the collider objects for the VehicleComponent. These colliders are used in determing center of mass for the vehicle.
        /// </summary>
        /// <param name="meshes">The meshes which are to represent the colliders, which will be invisible.</param>
        protected void BuildCollidersFrom(List<MeshRenderer> meshes)
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
                vehicle.gameObject.AddComponent<MeshCollider>().convex = true;
            }

        }
    }
}