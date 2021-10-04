To use the editor script VehicleWizard with a blender model:

MathUtilities.wheelQuantity can be set to reflect the number of wheels you desire. Model must reflect that same quantity.

a. Vehicle model must exist at the very top of the hierarchy. All other objects part of the vehicle setup must be children of this.
b. & c. Camera orbiters must have the word "orbiter" in its name and have a camera as a child node, offset from the orbiter by some distance and facing the vehicle.
d. Chassis wheel meshes must be children of a centered empty object.
e. Each wheel must have an empty parent situated and oriented where it is to be positioned on the vehicle. Each wheel must contain the string "motorized" and/or "steering" if motorized and/or a steering wheel.

f. & .g & h. & i. Colliders must have the word "Collider" in their name (case does matter).

j. Engine and drivetrain components must be named assembled together and appropriately named. Parented beneath the 
MyVehicleModel you will assign an empty node with the words "drivetrain" in it.



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

j. 	>DriveTrain
		>Engine
		>Transmission
		>Rack & Pinion
		>Exhaust

Design guidelines: 
>Drivetrain does not move locally.
>Engine powers the transmission AND vibrates with operation.
>Transmission feeds power to the wheels and  back to the engine. Operating just like a real transmission would (clutch, shifting, etc.). Rear axle has differential and propeller shaft if AWD/4WD.
>Rack & Pinion drives steering at the wheels (with steering wheels attached beneath this node)
>Exhaust gives off light smoke and vibrates with engine use and vehicle operation.