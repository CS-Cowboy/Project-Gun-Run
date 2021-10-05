To use the editor script VehicleWizard.cs with a blender model:

a. Vehicle model must exist at the very top of the hierarchy. All other objects part of the vehicle setup must be children of this.
b. & c. Camera orbiters must have the word "orbiter" in its name and have a camera as a child node, offset from the orbiter by some distance and facing the vehicle.
d. Chassis wheel meshes must be children of a centered empty object.
e. Each wheel must have an empty parent situated and oriented where it is to be positioned on the vehicle. Each wheel must contain the string "motorized" and/or "steering" if motorized and/or a steering wheel.

f. & .g & h. & i. Colliders must have the word "Collider" in their name (case does matter).

j. Vehicle components must be named assembled together and appropriately named. Parented beneath the 
MyVehicleModel you will assign an empty node and named "Components", each component will have a name containing "component" and its respective purpose (ie. transmissino, engine, differential, etc.).



To demonstrate said hierarchy:

a. >MyVehicleModel
b. 	>Orbiters
c.		>MyPlayerCamera
d.	>Chassis
e.		>Wheel_front_left
			>Wheel_mesh
		>Wheel_front_right
			>Wheel_mesh
		>Wheel_rear_left
			>Wheel_mesh
		>Wheel_rear_right
			>Wheel_mesh
f.	>Colliders
g.		>Collider_1
h. 		>Collider_2
i. 		>Collider_x

j. 	>Components
		>Engine_component
		>Transmissio_componentn
		>Rack & Pinion_component
		>Exhaust_component
		>Desired_component_with_existing_script
