#
#
#		IMPORTANT!
'		
'		FIRST:
'		
'		What are those huge warnings???
'		This version of HerdSim requires 2 layers for better physics handling. Better performance.
'		
#		Creating layers:
'		HerdSim Tutorial: 	https://www.youtube.com/watch?v=uajRWvddTe4
#		Unity Manual: 		http://docs.unity3d.com/Manual/Layers.html
#
'		Please create layers
'			Layer25: Ground
'			Layer26: HerdSim
'			
'		By default those are the layers used in index 25 and 26. Using other indexes is possible, but requires changing prefabs to those indexes.
'		Changing to another index will require changing prefabs and game objects as well.
'		Not a huge task, but if possible try to place Ground and HerdSim in layer index 25 and 26.
'		
'
'
'		SECOND (optional):
'	
'		move Gizmos folder to the root of your project. (Unity requires this folder in root to be able to show icons)
'
'
'
#		To use a custom model:
#		Duplicate the example pig prefab included.
#		Drag the prefab to the stage.
#		Drag the custom model to stage and replace the model object. Make sure it is rotated in the same direction as the old model.
#		Move the Scanner gameobject from old Model to the new model. Rotation of Scanner object should be 0,0,0
#		Delete the old model from the gameobject.
#		Assign new model to the _model variable.
#		Assign the right animation names.
#		Adjust collider if needed.
#		Adjust Avoidance and push variables if needed.
#		Apply changes.
#
#		Hierarchy of HerdSim object:
#		-Prefab
#				-Model			- Model with animations, bones and materials. Default animation names: walk, run, sleep, idle and dead
#				-Scanner		- Rotates to check for collisions, pushes away from objects close by. To avoid using rigidbodies for collisions, this method proves alot more CPU friendly.
#				-Collider		- Rays will be shot from the pivot point of this gameobject, make sure the pivot is inside the collider bounds. Or else ray might hit it's own collider.
#
#
#
#
#		More in depth info can be found in the script comments.
'
'
'		Support and suggestions are very welcome at support@chemicalbliss.com