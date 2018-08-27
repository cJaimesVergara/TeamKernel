//	Unluck Software	
// 	www.chemicalbliss.com
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HerdSimCore))]
[CanEditMultipleObjects]

[System.Serializable]
public class HerdSimCoreEditor : Editor {
	public bool showHelp;
	public bool showHelpComponents;
	public bool showHelpProperties;
	public bool showHelpAvoidance;
	public bool showHelpPush;
	public bool showHelpMovement;
	public bool showHelpGround;
	public bool showHelpHerding;
	public bool showHelpDeath;
	public bool showHelpAnimation;

	//Objects that can be multi edited
	public SerializedProperty groundLayerMask;
	public SerializedProperty updateDivisor;
	public SerializedProperty controller;
	public SerializedProperty hitPoints;
	public SerializedProperty type;
	public SerializedProperty minSize;
	public SerializedProperty avoidAngle;
	public SerializedProperty avoidDistance;
	public SerializedProperty avoidSpeed;
	public SerializedProperty stopDistance;
	public SerializedProperty pushHalfTheTime;
	public SerializedProperty pushDistance;
	public SerializedProperty pushForce;
	public SerializedProperty roamingArea;
	public SerializedProperty walkSpeed;
	public SerializedProperty runSpeed;
	public SerializedProperty damping;
	public SerializedProperty idleProbablity;
	public SerializedProperty runChance;
	public SerializedProperty groundCheckInterval;
	public SerializedProperty groundTag;
	public SerializedProperty maxGroundAngle;
	public SerializedProperty maxFall;
	public SerializedProperty fakeGravity;
	public SerializedProperty leaderAreaMultiplier;
	public SerializedProperty maxHerdSize;
	public SerializedProperty minHerdSize;
	public SerializedProperty herdDistance;
	public SerializedProperty randomDeath;
	public SerializedProperty deadMaterial;
	public SerializedProperty scaryCorpse;
	public SerializedProperty animIdle;
	public SerializedProperty animIdleSpeed;
	public SerializedProperty animSleep;
	public SerializedProperty animSleepSpeed;
	public SerializedProperty animWalk;
	public SerializedProperty animWalkSpeed;
	public SerializedProperty animRun;
	public SerializedProperty animRunSpeed;
	public SerializedProperty animDead;
	public SerializedProperty animDeadSpeed;
	public SerializedProperty idleToSleepSeconds;
	public SerializedProperty herdLayerMask;
	public SerializedProperty pushyLayerMask;
	public SerializedProperty herdSimLayerName;

	public SerializedProperty leanRightAnimation;
	public SerializedProperty leanLeftAnimation;

	public static Texture2D tex;

	public void OnEnable() {
		pushyLayerMask = serializedObject.FindProperty("_pushyLayerMask");
		herdSimLayerName = serializedObject.FindProperty("_herdSimLayerName");
		herdLayerMask = serializedObject.FindProperty("_herdLayerMask");
		animIdle = serializedObject.FindProperty("_animIdle");
		animIdleSpeed = serializedObject.FindProperty("_animIdleSpeed");
		animSleep = serializedObject.FindProperty("_animSleep");
		animSleepSpeed = serializedObject.FindProperty("_animSleepSpeed");
		animWalk = serializedObject.FindProperty("_animWalk");
		animWalkSpeed = serializedObject.FindProperty("_animWalkSpeed");
		animRun = serializedObject.FindProperty("_animRun");
		animRunSpeed = serializedObject.FindProperty("_animRunSpeed");
		animDead = serializedObject.FindProperty("_animDead");
		animDeadSpeed = serializedObject.FindProperty("_animDeadSpeed");
		idleToSleepSeconds = serializedObject.FindProperty("_idleToSleepSeconds");
		deadMaterial = serializedObject.FindProperty("_deadMaterial");
		scaryCorpse = serializedObject.FindProperty("_scaryCorpse");
		leaderAreaMultiplier = serializedObject.FindProperty("_leaderAreaMultiplier");
		maxHerdSize = serializedObject.FindProperty("_maxHerdSize");
		minHerdSize = serializedObject.FindProperty("_minHerdSize");
		herdDistance = serializedObject.FindProperty("_herdDistance");
		groundCheckInterval = serializedObject.FindProperty("_groundCheckInterval");
		groundTag = serializedObject.FindProperty("_groundTag");
		maxGroundAngle = serializedObject.FindProperty("_maxGroundAngle");
		maxFall = serializedObject.FindProperty("_maxFall");
		fakeGravity = serializedObject.FindProperty("_fakeGravity");
		roamingArea = serializedObject.FindProperty("_roamingArea");
		walkSpeed = serializedObject.FindProperty("_walkSpeed");
		runSpeed = serializedObject.FindProperty("_runSpeed");
		damping = serializedObject.FindProperty("_damping");
		idleProbablity = serializedObject.FindProperty("_idleProbablity");
		runChance = serializedObject.FindProperty("_runChance");
		groundLayerMask = serializedObject.FindProperty("_groundLayerMask");
		updateDivisor = serializedObject.FindProperty("_updateDivisor");
		controller = serializedObject.FindProperty("_controller");
		hitPoints = serializedObject.FindProperty("_hitPoints");
		type = serializedObject.FindProperty("_type");
		minSize = serializedObject.FindProperty("_minSize");
		avoidAngle = serializedObject.FindProperty("_avoidAngle");
		avoidDistance = serializedObject.FindProperty("_avoidDistance");
		avoidSpeed = serializedObject.FindProperty("_avoidSpeed");
		stopDistance = serializedObject.FindProperty("_stopDistance");
		pushHalfTheTime = serializedObject.FindProperty("_pushHalfTheTime");
		pushDistance = serializedObject.FindProperty("_pushDistance");
		pushForce = serializedObject.FindProperty("_pushForce");
		leanRightAnimation = serializedObject.FindProperty("_leanRightAnimation");
		leanLeftAnimation = serializedObject.FindProperty("_leanLeftAnimation");
	}

	public override void OnInspectorGUI() {
		var target_cs = (HerdSimCore)target;
		serializedObject.Update();
		Color dColor = new Color32((byte)175, (byte)175, (byte)175, (byte)255);
		Color aColor = Color.white;
		Color helpColor = Color.yellow;
		Color warningColor =    new Color32((byte)255, (byte)174, (byte)0, (byte)255);
		Color warningColor2 =  Color.yellow;
		GUIStyle helpStyle = null;
		GUIStyle buttonStyle = null;
		GUIStyle boxStyle = null;

		// GUI.color = bColor;
		helpStyle = new GUIStyle(GUI.skin.label);
		helpStyle.fontSize = 9;
		helpStyle.normal.textColor = helpColor;

		GUIStyle warningStyle = new GUIStyle(GUI.skin.label);
		warningStyle.normal.textColor = warningColor;
		warningStyle.fontStyle = FontStyle.Bold;

		GUIStyle warningStyle2 = new GUIStyle(GUI.skin.label);
		warningStyle2.normal.textColor = warningColor2;
		warningStyle2.fontStyle = FontStyle.Bold;

		buttonStyle = new GUIStyle(GUI.skin.button);
		buttonStyle.fontStyle = FontStyle.Bold;
		buttonStyle.fixedWidth = 25.0f;

		boxStyle = new GUIStyle(GUI.skin.box);
		boxStyle.stretchWidth = true;
		boxStyle.fontStyle = FontStyle.Bold;
		boxStyle.normal.textColor = warningColor;
		boxStyle.normal.background = tex;

		GUIStyle boxStyle2 = new GUIStyle(boxStyle);
		boxStyle2.normal.textColor = warningColor2;
		boxStyle2.margin = new RectOffset(0, 0, 0, 0);
		GUIStyle boxStyle3 = new GUIStyle(boxStyle2);
		boxStyle3.fontStyle = FontStyle.Normal;
		boxStyle3.fontSize = 9;

		GUILayout.Space(5.0f);

		bool warned = false;

		if (LayerMask.NameToLayer(target_cs._groundTag) == -1) {
			EditorGUILayout.LabelField("Warning: No " + target_cs._groundTag + " layer found", boxStyle);
			warned = true;
		}


		if (LayerMask.NameToLayer(target_cs._herdSimLayerName) == -1) {
			EditorGUILayout.LabelField("Warning: No " + target_cs._herdSimLayerName + " layer found", boxStyle);
			warned = true;
		}

		if (warned) {
			EditorGUILayout.LabelField("Please create layers:\nLayer25: Ground\n & \nLayer26: HerdSim\n\nSee Readme.txt for more info", boxStyle2);
			EditorGUILayout.LabelField("This can must be done manually", boxStyle3);
			if (tex == null) {
				tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
				tex.SetPixel(0, 0, new Color32((byte)0, (byte)0, (byte)0, (byte)255));
				tex.hideFlags = HideFlags.DontSave;
				tex.Apply();
			}

		}
		GUI.color = dColor;
		EditorGUILayout.BeginVertical("Box");

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Frame Skipping", EditorStyles.boldLabel);


		GUI.color = helpColor; if (GUILayout.Button("?", buttonStyle)) {
			showHelp = !showHelp;
		}


		EditorGUILayout.EndHorizontal(); GUI.color = aColor;
		EditorGUILayout.IntSlider(updateDivisor, 1, 9);

		if (showHelp) EditorGUILayout.LabelField("Increase performance by skipping frames", helpStyle);
		if (target_cs._updateDivisor > 4) {
			//  GUI.color = wColor;

			EditorGUILayout.LabelField("Will cause choppy movement", warningStyle);

		} else if (target_cs._updateDivisor > 2) {
			//  GUI.color = w2Color;

			EditorGUILayout.LabelField("Can cause choppy movement	", warningStyle2);

		}


		EditorGUILayout.EndVertical();

		GUI.color = dColor;
		EditorGUILayout.BeginVertical("Box");

		EditorGUILayout.BeginHorizontal();
		GUI.color = aColor;
		target_cs._lean = EditorGUILayout.Toggle("", target_cs._lean, GUILayout.Width(10.0f));
		GUI.color = dColor;

		EditorGUILayout.LabelField("Leaning", EditorStyles.boldLabel);
		GUI.color = aColor;
		EditorGUILayout.EndHorizontal();

		if (target_cs._lean) {
			if (showHelpComponents) EditorGUILayout.LabelField("Blend animation to the right when moving right", helpStyle);

			target_cs._leanRightAnimation = EditorGUILayout.ObjectField("Lean Right Animation", target_cs._leanRightAnimation, typeof(AnimationClip), true) as AnimationClip;
			if (showHelpComponents) EditorGUILayout.LabelField("Blend animation to the right when moving right", helpStyle);

			target_cs._leanLeftAnimation = EditorGUILayout.ObjectField("Lean Left Animation", target_cs._leanLeftAnimation, typeof(AnimationClip), true) as AnimationClip;
			if (showHelpComponents) EditorGUILayout.LabelField("Blend animation to the left when moving left", helpStyle);

		}
		EditorGUILayout.EndVertical();

		GUI.color = dColor;
		EditorGUILayout.BeginVertical("Box");


		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Components", EditorStyles.boldLabel);
		GUI.color = helpColor; if (GUILayout.Button("?", buttonStyle)) {
			showHelpComponents = !showHelpComponents;
		}
		EditorGUILayout.EndHorizontal(); GUI.color = aColor;



		EditorGUILayout.PropertyField(controller, new GUIContent("Controller"));
		if (showHelpComponents) EditorGUILayout.LabelField("Used to make shared roaming area", helpStyle);

		target_cs._scanner = EditorGUILayout.ObjectField("Scanner", target_cs._scanner, typeof(Transform), true) as Transform;
		if (showHelpComponents) EditorGUILayout.LabelField("Used for push; rotates to check for collisions", helpStyle);

		target_cs._collider = EditorGUILayout.ObjectField("Collider", target_cs._collider, typeof(Transform), true) as Transform;
		if (showHelpComponents) EditorGUILayout.LabelField("Rays are shot from within the collider pivot", helpStyle);

		target_cs._model = EditorGUILayout.ObjectField("Model", target_cs._model, typeof(Transform), true) as Transform;
		if (showHelpComponents) EditorGUILayout.LabelField("Animated model", helpStyle);

		target_cs._renderer = EditorGUILayout.ObjectField("Renderer", target_cs._renderer, typeof(Renderer), true) as Renderer;
		if (showHelpComponents) EditorGUILayout.LabelField("Model renderer, used to swap material when this dies", helpStyle);




		EditorGUILayout.EndVertical();
		GUI.color = dColor;
		EditorGUILayout.BeginVertical("Box");



		//PROPERTIES

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Properties", EditorStyles.boldLabel);
		GUI.color = helpColor; if (GUILayout.Button("?", buttonStyle)) {
			showHelpProperties = !showHelpProperties;
		}
		EditorGUILayout.EndHorizontal(); GUI.color = aColor;

		EditorGUILayout.PropertyField(hitPoints, new GUIContent("Hit Points"));
		if (showHelpProperties) {
			EditorGUILayout.LabelField("Life points; when below zero this dies", helpStyle);
			EditorGUILayout.LabelField("Example: HerdSimCore.Damage(5);", helpStyle);
		}
		EditorGUILayout.PropertyField(type, new GUIContent("Type"));
		if (showHelpProperties) EditorGUILayout.LabelField("Creates herds with others of the same type", helpStyle);

		EditorGUILayout.PropertyField(minSize, new GUIContent("Minimum Size"));
		if (showHelpProperties) EditorGUILayout.LabelField("Randomzed scale, minimum to 1", helpStyle);


		EditorGUILayout.EndVertical();
		GUI.color = dColor;
		EditorGUILayout.BeginVertical("Box");



		///AVOIDANCE

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Avoidance", EditorStyles.boldLabel);
		GUI.color = helpColor; if (GUILayout.Button("?", buttonStyle)) {
			showHelpAvoidance = !showHelpAvoidance;
		}
		EditorGUILayout.EndHorizontal(); GUI.color = aColor;


		EditorGUILayout.PropertyField(avoidAngle, new GUIContent("Avoid Angle"));
		if (showHelpAvoidance) EditorGUILayout.LabelField("Angle of the rays used to avoid obstacles", helpStyle);

		EditorGUILayout.PropertyField(avoidDistance, new GUIContent("Avoid Distance"));
		if (showHelpAvoidance) EditorGUILayout.LabelField("How far avoid rays travel", helpStyle);

		EditorGUILayout.PropertyField(avoidSpeed, new GUIContent("Avoid Speed"));
		if (showHelpAvoidance) EditorGUILayout.LabelField("How fast this turns around when avoiding	", helpStyle);

		EditorGUILayout.PropertyField(stopDistance, new GUIContent("Stop Distance"));
		if (showHelpAvoidance) EditorGUILayout.LabelField("Proximity to collider in front before stopping", helpStyle);



		EditorGUILayout.EndVertical();
		GUI.color = dColor;
		EditorGUILayout.BeginVertical("Box");



		///PUSH

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Push / Collision", EditorStyles.boldLabel);
		GUI.color = helpColor; if (GUILayout.Button("?", buttonStyle)) {
			showHelpPush = !showHelpPush;
		}
		EditorGUILayout.EndHorizontal(); GUI.color = aColor;

		EditorGUILayout.PropertyField(herdSimLayerName, new GUIContent("HerdSim Layer Name"));
		if (showHelpPush) EditorGUILayout.LabelField("Layer name of herdsim animals (Default:HerdSim)", helpStyle);

		EditorGUILayout.PropertyField(pushyLayerMask, new GUIContent("Pushy Layer Mask"));
		if (showHelpPush) EditorGUILayout.LabelField("Push and Avoidance will avoid colliders in layers", helpStyle);


		EditorGUILayout.PropertyField(pushHalfTheTime, new GUIContent("Push half the time"));
		if (showHelpPush) EditorGUILayout.LabelField("Reduce how many times push checks for obstacles", helpStyle);

		EditorGUILayout.PropertyField(pushDistance, new GUIContent("Push Distance"));
		if (showHelpPush) EditorGUILayout.LabelField("How far away from obstacles before push", helpStyle);

		EditorGUILayout.PropertyField(pushForce, new GUIContent("Push Force"));
		if (showHelpPush) EditorGUILayout.LabelField("How fast/hard to push away", helpStyle);

		EditorGUILayout.EndVertical();
		GUI.color = dColor;
		EditorGUILayout.BeginVertical("Box");



		///MOVEMENT

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Movement", EditorStyles.boldLabel);
		GUI.color = helpColor; if (GUILayout.Button("?", buttonStyle)) {
			showHelpMovement = !showHelpMovement;
		}
		EditorGUILayout.EndHorizontal(); GUI.color = aColor;

		if (target_cs._controller == null) {
			EditorGUILayout.PropertyField(roamingArea, new GUIContent("Roaming Area"));
			if (showHelpMovement) EditorGUILayout.LabelField("The area this roams within", helpStyle);
		}
		EditorGUILayout.PropertyField(walkSpeed, new GUIContent("Walk Speed"));
		if (showHelpMovement) EditorGUILayout.LabelField("How fast this moves while walking", helpStyle);

		EditorGUILayout.PropertyField(runSpeed, new GUIContent("Run Speed"));
		if (showHelpMovement) EditorGUILayout.LabelField("How fast this moves while running", helpStyle);

		EditorGUILayout.PropertyField(damping, new GUIContent("Turn Speed"));
		if (showHelpMovement) EditorGUILayout.LabelField("How quickly this turns", helpStyle);

		EditorGUILayout.PropertyField(idleProbablity, new GUIContent("Idle Probablity"));
		if (showHelpMovement) EditorGUILayout.LabelField("Chance this will stop instead of finding a new waypoint", helpStyle);
		if (showHelpMovement) EditorGUILayout.LabelField("0-100 chance every second while idle", helpStyle);

		EditorGUILayout.PropertyField(runChance, new GUIContent("Run Chance"));
		if (showHelpMovement) EditorGUILayout.LabelField("If not idle % chance this will run instead of walk", helpStyle);



		EditorGUILayout.EndVertical();
		GUI.color = dColor;
		EditorGUILayout.BeginVertical("Box");




		///GROUND

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Ground", EditorStyles.boldLabel);
		GUI.color = helpColor; if (GUILayout.Button("?", buttonStyle)) {
			showHelpGround = !showHelpGround;
		}


		EditorGUILayout.EndHorizontal(); GUI.color = aColor;



		EditorGUILayout.LabelField("Update: Ground detection changed from Tag to Layer", helpStyle);
		EditorGUILayout.PropertyField(groundTag, new GUIContent("Ground Layer Name"));
		if (showHelpGround) EditorGUILayout.LabelField("The ground should have this layer (Default: Ground)", helpStyle);

		EditorGUILayout.Slider(groundCheckInterval, 0.025f, 1.0f, new GUIContent("Ground Check Interval"));

		if (showHelpGround) EditorGUILayout.LabelField("How often to check for ground (seconds)", helpStyle);

		EditorGUILayout.PropertyField(groundLayerMask, new GUIContent("Ground Layer Mask"), true);
		if (showHelpGround) EditorGUILayout.LabelField("Layers this can walk on", helpStyle);

		EditorGUILayout.PropertyField(maxGroundAngle, new GUIContent("Max Ground Angle"));
		if (showHelpGround) EditorGUILayout.LabelField("Maximum angle this will walk up", helpStyle);

		EditorGUILayout.PropertyField(maxFall, new GUIContent("Max Fall"));
		if (showHelpGround) EditorGUILayout.LabelField("Max distance to find new ground position", helpStyle);

		EditorGUILayout.PropertyField(fakeGravity, new GUIContent("Fake Gravity"));
		if (showHelpGround) EditorGUILayout.LabelField("How fast this will move towards the ground", helpStyle);




		EditorGUILayout.EndVertical();
		GUI.color = dColor;
		EditorGUILayout.BeginVertical("Box");




		///HERDING

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Herding", EditorStyles.boldLabel);
		GUI.color = helpColor; if (GUILayout.Button("?", buttonStyle)) {
			showHelpHerding = !showHelpHerding;
		}
		EditorGUILayout.EndHorizontal(); GUI.color = aColor;

		EditorGUILayout.PropertyField(herdLayerMask, new GUIContent("Animal Layers"));
		if (showHelpHerding) EditorGUILayout.LabelField("Layer containing HerdSim animals", helpStyle);

		EditorGUILayout.PropertyField(leaderAreaMultiplier, new GUIContent("Leader Area Multiplier"));
		if (showHelpHerding) EditorGUILayout.LabelField("How big the leader area grows for each follower", helpStyle);

		EditorGUILayout.PropertyField(maxHerdSize, new GUIContent("Herd Size"));
		if (showHelpHerding) EditorGUILayout.LabelField("Minimum amount of followers", helpStyle);

		EditorGUILayout.PropertyField(minHerdSize, new GUIContent("Min Herd Size"));
		if (showHelpHerding) EditorGUILayout.LabelField("Minimum amount of followers", helpStyle);

		EditorGUILayout.PropertyField(herdDistance, new GUIContent("Herd Distance"));
		if (showHelpHerding) EditorGUILayout.LabelField("How far this will check for a herd", helpStyle);



		EditorGUILayout.EndVertical();
		GUI.color = dColor;
		EditorGUILayout.BeginVertical("Box");



		///DEATH
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Death", EditorStyles.boldLabel);
		GUI.color = helpColor; if (GUILayout.Button("?", buttonStyle)) {
			showHelpDeath = !showHelpDeath;
		}
		EditorGUILayout.EndHorizontal(); GUI.color = aColor;

		EditorGUILayout.PropertyField(deadMaterial, new GUIContent("Dead Material"));
		if (showHelpDeath) EditorGUILayout.LabelField("Material to apply when this dies", helpStyle);

		EditorGUILayout.PropertyField(scaryCorpse, new GUIContent("Scary Corpse"));
		if (showHelpDeath) EditorGUILayout.LabelField("Corpse scares others", helpStyle);



		EditorGUILayout.EndVertical();
		GUI.color = dColor;
		EditorGUILayout.BeginVertical("Box");



		///ANIMATION
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Animation", EditorStyles.boldLabel);
		GUI.color = helpColor; if (GUILayout.Button("?", buttonStyle)) {
			showHelpAnimation = !showHelpAnimation;
		}

		EditorGUILayout.EndHorizontal(); GUI.color = aColor;


		EditorGUILayout.PropertyField(animIdle, new GUIContent("Idle"), true);
		if (showHelpAnimation) EditorGUILayout.LabelField("Idle animation name", helpStyle);

		EditorGUILayout.PropertyField(animIdleSpeed, new GUIContent("Idle Speed"), true);
		EditorGUILayout.PropertyField(animSleep, new GUIContent("Sleep"), true);
		EditorGUILayout.PropertyField(animSleepSpeed, new GUIContent("Sleep Speed"), true);
		EditorGUILayout.PropertyField(animWalk, new GUIContent("Walk"), true);
		EditorGUILayout.PropertyField(animWalkSpeed, new GUIContent("alk Speed"), true);
		EditorGUILayout.PropertyField(animRun, new GUIContent("Run"), true);
		EditorGUILayout.PropertyField(animRunSpeed, new GUIContent("Run Speed"), true);
		EditorGUILayout.PropertyField(animDead, new GUIContent("Dead"), true);
		EditorGUILayout.PropertyField(animDeadSpeed, new GUIContent("Dead Speed"), true);
		EditorGUILayout.PropertyField(idleToSleepSeconds, new GUIContent("Seconds to sleep"), true);
		if (showHelpAnimation) EditorGUILayout.LabelField("Time it takes to fall asleep", helpStyle);
		if (showHelpAnimation) EditorGUILayout.LabelField("(or change to another idle)", helpStyle);

		EditorGUILayout.EndVertical();

		GUI.color = helpColor; if (GUILayout.Button("Add HerdSimDisabler Script")) {

			for (int i = 0; i < Selection.gameObjects.Length; i++) {
				HerdSimDisabler h = null;
				h = Selection.gameObjects[i].GetComponent<HerdSimDisabler>();
				if (h == null)
					Selection.gameObjects[i].AddComponent<HerdSimDisabler>();
			}
		}
		if (GUI.changed) EditorUtility.SetDirty(target_cs);
		serializedObject.ApplyModifiedProperties();
	}
}