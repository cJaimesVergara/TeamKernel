using UnityEngine;
using System;


public class HerdSimCore:MonoBehaviour{
	///HERDSIM is a simple and easy to use animal roaming simulator.
	///Simply drag the prefab to the stage and it will move within the roaming area.
	///Assign a tag to the ground. (Default tag name: "Ground")
	
	///To use a custom model:
	///Duplicate the example pig prefab included.
	///Drag the prefab to the stage.
	///Drag the custom model to stage and replace the model object. Make sure it is rotated in the same direction as the old model.
	///Move the Scanner gameobject from old Model to the new model. Rotation of Scanner object should be 0,0,0
	///Delete the old model from the gameobject.
	///Assign new model to the _model variable.
	///Assign the right animation names.
	///Adjust collider if needed.
	///Adjust Avoidance and push variables if needed.
	///Apply changes.
	
	///Hierarchy of HerdSim object:
	///-Prefab
	///		-Model			- Model with animations, bones and materials. Default animation names: walk, run, sleep, idle and dead
	///			-Scanner	- Rotates to check for collisions, pushes away from objects close by. To avoid using rigidbodies for collisions, this method proves alot more CPU friendly.
	///		-Collider		- Rays will be shot from the pivot point of this gameobject, make sure the pivot is inside the collider bounds. Or else ray might hit it's own collider.
	///
	
	///OBJECTS
	public HerdSimController _controller;	//Controller is used to make one big roaming area
	public Transform _scanner;				//Scanner object used for push, this rotates to check for collisions
	public Transform _collider;				//Collider object, ground check uses _collider position to find ground. Ray is shot from within the collider to avoid hitting this collider
	public Transform _model;				//Model object, must be parented within this transform and contain the required animations.
	public Renderer _renderer;				//Renderer of the model object, used to swap material when this dies
	
	
	///PROPERTIES
	public float _hitPoints = 100.0f;			//Life points, below zero and this dies. Use the Damage(x); function to decrease hitpoints: example : SendMessage ("Damage", 5.0);
	public int _type;						//This will only create herds with others of the same type
	public float _minSize = 1.0f;				//Randomzed scale, minimum. (only scales down to avoid clipping, this walking trough others)
	
	
	///AVOIDANCE
	public float _avoidAngle = 0.35f; 		//Angle of the rays used to avoid obstacles left and right
	public float _avoidDistance;			//How far avoid rays travel
	public float _avoidSpeed = 75.0f;			//How fast this turns around when avoiding	
	public float _stopDistance;				//How close this can be to objects directly in front of it before stopping and backing up. This will also rotate slightly, to avoid "robotic" behaviour
	float _rotateCounterR;			//Used to increase avoidance speed over time
	float _rotateCounterL;
	
	
	///PUSH (COLLISIONS) - Push is used to simulate collisions, a ray is used to scan for close by objects and push this away
	public bool _pushHalfTheTime;
	bool _pushToggle;
	public float _pushDistance;				//How far away obstacles can be before starting to push away	
	public float _pushForce = 5.0f;			//How fast/hard to push away
	bool _scan;					//Scanner rotate fast or slow
	
	
	///MOVEMENT
	public Vector3 _roamingArea;			//The area this roams within, this will not be used if a herd controller is assigned
	public float _walkSpeed = .5f;			//The speed of this while walking
	public float _runSpeed = 1.5f;			//Running speed
	public float _damping;					//How fast this should turn towards waypoint, avay from obstacles or scary objects
	public int _idleProbablity = 20;		//Chance this will stop instead of finding a new waypoint 20 is aprox 20 to 1 chance every second standing still.
	public float _runChance = .1f;			//If not idle % chance this will run instead of walk (only applies to leaders or individuals without a herd)
	
	
	public Vector3 _waypoint;						//Waypoint this should rotate towards
	public float _speed;							//Current speed this has
	public float _targetSpeed;						//The desired speed
	public int _mode = 0;							//The movement state of this, 0 = idle 1 = walk 3 = run
	public Vector3 _startPosition;					//The position this was when it was added to the scene, used for roaming area when no herd controller is assigned. Change this to move roaming area when not using controller										
	bool _reachedWaypoint = true;		//If this can get a new waypoint to move to
	int _lerpCounter;						//Used to rotate smoothly
	
	
	///FEAR
	public bool _scared;				//If this is scared it will run and flee away from _scareOf
	public Transform _scaredOf;
	
	
	///FOOD
	public bool _eating;				
	public Transform _food;
	
	
	///GROUND
	public float _groundCheckInterval = 0.1f;	//How often to check where the ground is
	public string _groundTag = "Ground";		//The ground should have this tag so that this knows how to behave if it collides with it (not avoid ground)
	Vector3 _ground;					//Position of last ground position
	Quaternion _groundRot;				//The rotation used on the model to align with the grounds angle
	bool _grounded;
	public float _maxGroundAngle = 45.0f;		//Maximum angle this will walk up
	public float _maxFall = 3.0f;				//Max distance to find new ground position, used to avoid falling down from heights. It is better to use invisible colliders.
	public float _fakeGravity = 5.0f;			//How fast this will move towards the ground. This works on both up and down directions smoothly.
	
	
	///HERD
	public LayerMask _herdLayerMask = (LayerMask)(-1);					//Mask to look for other animals
	public HerdSimCore _leader;						//Who is the leader of the pack, can be this
	public Vector3 _leaderArea;						//When this is a leader, this are will be used by all followers
	public int _leaderSize;							//How many followers this has
	public float _leaderAreaMultiplier = .2f;		//How big the leader area grows for each follower
	public int _maxHerdSize = 25;					//Max amount of followers
	public int _minHerdSize = 10;					//Min ---
	public float _herdDistance = 2.0f;					//How far this will check for a herd
	int _herdSize;							//Random Min/Max followers this can have
	
	
	///DEATH
	public bool _dead;							//This is dead or not
	public Material _deadMaterial;					//Material to apply when this dies
	public bool _scaryCorpse;					//Corpse scares others
	
	
	///ANIMATIONS
	public string _animIdle = "idle";
	public float _animIdleSpeed = 1.0f;
	public string _animSleep = "sleep";
	public float _animSleepSpeed = 1.0f;
	public string _animWalk = "walk";
	public float _animWalkSpeed = 1.0f;
	public string _animRun = "run";
	public float _animRunSpeed = 1.0f;
	public string _animDead = "dead";
	public float _animDeadSpeed = 1.0f;
	public float _idleToSleepSeconds = 1.0f;
	
	float _sleepCounter;
	bool _idle;						//Used for idle animations
	
	///FRAME SKIP
	int _updateCounter;					//
	public int _updateDivisor = 1;				//Skip update every N frames (Higher numbers might give choppy results, 3 - 4 on 60fps , 2 - 3 on 30 fps)
	static int _updateNextSeed = 0;		
	int _updateSeed = -1;	
	float _newDelta;
	
	///ENABLE/DISABLE
	public bool _enabled;
	
	public LayerMask _groundLayerMask = (LayerMask)(-1);
	public LayerMask _pushyLayerMask = (LayerMask)(-1);
	public string _herdSimLayerName = "HerdSim";
	
	int _groundIndex = 25;
	int _herdSimIndex = 26;
	
	Transform _thisTR;


	public bool _lean;
	public AnimationClip _leanLeftAnimation;
	public AnimationClip _leanRightAnimation;

	AnimationState _leanLeft;
	AnimationState _leanRight;

	float _leanRightTime;
	float _leanLeftTime;

	bool _avoiding;

	bool _avoidingLeft;
	bool _avoidingRight;

	public void Start() {
		_thisTR = transform;
		_enabled = true;
		_groundIndex = LayerMask.NameToLayer(this._groundTag);
		_herdSimIndex = LayerMask.NameToLayer(this._herdSimLayerName);

		if (_updateDivisor > 1) {
			int _updateSeedCap = _updateDivisor - 1;
			_updateNextSeed++;
			this._updateSeed = _updateNextSeed;
			_updateNextSeed = _updateNextSeed % _updateSeedCap;
		}
		//GroundTag should not be null
		if (_groundTag == null)
			_groundTag = "Ground";

		Init();

		//Save starting position so that roaming area doesn't move
		_startPosition = _thisTR.position;

		//Makes sure push distance is greater than zero
		if (_pushDistance <= 0)
			_pushDistance = _avoidDistance * .25f;

		//Makes sure stopping distance is greater than zero
		if (_stopDistance <= 0)
			_stopDistance = _avoidDistance * .25f;

		//Set ground and waypoint to this position
		_ground = _waypoint = _thisTR.position;

		//Ignores max fall first time looking for ground
		float b = _maxFall;
		_maxFall = 1000000.0f;
		GroundCheck();
		_maxFall = b;

		if (_collider == null)
			_collider = _thisTR.Find("Collider");

		//Randomize herd size when this is leader
		_herdSize = UnityEngine.Random.Range(this._minHerdSize, this._maxHerdSize);

		//Randomize transform size
		if (this._minSize < 1)
			this._thisTR.localScale = Vector3.one * UnityEngine.Random.Range(this._minSize, 1.0f);

		_model.GetComponent<Animation>()[this._animIdle].speed = _animIdleSpeed;
		_model.GetComponent<Animation>()[this._animDead].speed = _animDeadSpeed;
		_model.GetComponent<Animation>()[this._animSleep].speed = _animSleepSpeed;

		LeanInit();

	}

	void LeanInit() {
		if (!_lean) return;
		_leanLeft = _model.GetComponent<Animation>()[_leanLeftAnimation.name];
		_leanRight = _model.GetComponent<Animation>()[_leanRightAnimation.name];
		_leanRight.layer = _leanLeft.layer = 10;
		_leanRight.blendMode = _leanLeft.blendMode = AnimationBlendMode.Additive;
		_leanRight.enabled = true;
		_leanLeft.enabled = true;
		_leanLeft.weight = _leanRight.weight = 1.0f;
		_leanRight.wrapMode = _leanLeft.wrapMode = WrapMode.ClampForever;
	}

	void Lean() {
		if (!_lean) return;
		float a = AngleAmount();
		if (_avoidingLeft || !_avoiding && _mode != 0 && a < 0.3) _leanLeftTime = Mathf.Lerp(_leanLeftTime, (-a), _newDelta * 2f);
		else _leanLeftTime = Mathf.Lerp(_leanLeftTime, 0, _newDelta);
		if (_avoidingRight || !_avoiding && _mode != 0 && a > 0.3) _leanRightTime = Mathf.Lerp(_leanRightTime, (a), _newDelta * 2f);
		else _leanRightTime = Mathf.Lerp(_leanRightTime, 0, _newDelta);
		_leanLeft.normalizedTime = _leanLeftTime;
		_leanRight.normalizedTime = _leanRightTime;
	}

	public void Disable(bool disableModel,bool disableCollider){
		if(_enabled){
			_enabled = false;
			CancelInvoke();		
			if(disableModel)
			this._model.gameObject.SetActive(false);
			if(disableCollider)
			this._collider.gameObject.SetActive(false);
			_thisTR.GetComponent<HerdSimCore>().enabled = false;
			_model.GetComponent<Animation>().Stop();
		}
	}
	
	public void Enable(){
		if(!_enabled){
			_enabled = true;
			Init();
			if(!_model.gameObject.activeInHierarchy)
			this._model.gameObject.SetActive(true);
			if(!_collider.gameObject.activeInHierarchy)
			this._collider.gameObject.SetActive(true);
			_thisTR.GetComponent<HerdSimCore>().enabled = true;
			_model.GetComponent<Animation>().Play();
		}
	}
	
	public void Damage(float d){
		_hitPoints -= d;
		if(_hitPoints <=0){
			Death();
		}
	}
	
	public void Effects(){
		if((_controller != null)  && _mode == 2 && (_controller._runPS != null) && _speed > 1 ){
	    	_controller._runPS.transform.position = this._thisTR.position;
			_controller._runPS.Emit(1);
		}
		if(_dead && (_controller != null) && (_controller._deadPS != null)){
			_controller._deadPS.transform.position = _collider.transform.position;
			_controller._deadPS.Emit(1);
		}
	}
	
	public void Death(){
		if(!_dead){
			_dead = true;
			this._mode = 0;
			CancelInvoke("Wander");
			CancelInvoke("WalkTimeOut");
			CancelInvoke("FindLeader");
			if(_leader != null){
				if(_leader != this)	
				_leader._leaderSize--;
				else
				_leaderSize = 0;
				_leader = null;
			}
			if(_deadMaterial != null)
			_renderer.sharedMaterial = _deadMaterial;		
			_model.GetComponent<Animation>()[this._animDead].speed = 1.0f;   
			_model.GetComponent<Animation>().CrossFade(_animDead,.1f);
			if(_scaryCorpse)
			InvokeRepeating("Corpse", 1.0f, 1.0f);
		}
	}
	
	public void Corpse(){
		Collider[] hitColliders = Physics.OverlapSphere(_thisTR.position, 10.0f);
			HerdSimCore c = null;
			for(int i = 0; i < hitColliders.Length; i++) {
				if(hitColliders[i].transform.parent != null)
				c = hitColliders[i].transform.parent.GetComponent<HerdSimCore>();
				if(_scaryCorpse && (c != null) && !c._dead && c._mode<1){
					c.Scare(transform);
				}
			}
	}
	
	///HERD
	public void FindLeader(){
		if(_leader == this && _leaderSize <= 1){
			_leader = null;
			_leaderSize = 0;
		}else 	 
		if(_leader != this){
			if((_leader != null) && _leader._dead)
			_leader=null;
			_leaderSize = 0;
			Collider[] hitColliders = Physics.OverlapSphere(_thisTR.position, _herdDistance, _herdLayerMask);
			HerdSimCore c = null;
			for(int i = 0; i < hitColliders.Length; i++) {
				if(hitColliders[i].transform.parent != null)
				c = hitColliders[i].transform.parent.GetComponent<HerdSimCore>();
				if((c != null) && c!=this && _type == c._type){
					//Nobody has a leader, this becomes leader of other
					if((_leader == null) && (c._leader == null)){				
						_leader = this;
						c._leader = this;
						_leaderSize +=2;
						break;
					}						
					//If collider object has leader but this has null, leader is adopded -easy
					if((_leader == null) && (c._leader != null) && c._leader._leaderSize < c._leader._herdSize){			
						_leader = c._leader;
						_leader._leaderSize++;
						break;
					}
					if((_leader != null) && c._leader != _leader){
						if((c._leader != null) && c._leader._leaderSize >= _leader._leaderSize && c._leader._leaderSize < c._leader._herdSize){
							_leader._leaderSize--;
							c._leader._leaderSize++;
							_leader = c._leader;
							break;
						}
					}
				}
			}
		}
	}
	
	public void Wander(){
	
		Vector3 t = Vector3.zero;
			if(_leader == this)
			_leaderArea = Vector3.one * ((_leaderSize*_leaderAreaMultiplier)+1);
			Vector3 _ra = Vector3.zero;
			Vector3 _pb = Vector3.zero;
			if((_leader != null) && _leader!=this){
				_ra = _leader._leaderArea;
				_pb = _leader.transform.position;
			}else if(_controller == null){
				_ra = _roamingArea;
				_pb = _startPosition;
			}else{
				_ra = _controller._roamingArea;
				_pb = _controller.transform.position;
			}
			t.x = UnityEngine.Random.Range(-_ra.x, _ra.x) + _pb.x;
			t.z = UnityEngine.Random.Range(-_ra.z, _ra.z) + _pb.z;	
		if(_food != null){
			t = _food.position;
			_mode = 2;
			}else if(this!=null){
				//Check if this is inside the roaming area, if not run to next waypoint.
				if(_thisTR.position.x < -_ra.x + _pb.x 
				|| _thisTR.position.x > _ra.x + _pb.x
				|| _thisTR.position.z < -_ra.z + _pb.z
				|| _thisTR.position.z > _ra.z + _pb.z
				){	
				if(UnityEngine.Random.value < .1f){
					_mode = 2;	//Run
				}else{
					_mode = 1;	//Walk
				}
				_waypoint = t;
				}else if((_leader != null) && _leader != this && UnityEngine.Random.value < .75f){			
					_mode = 0;	//Stop			
				}else if(_reachedWaypoint){
					_mode = UnityEngine.Random.Range(-_idleProbablity , 2);		
					if(_mode == 1 && UnityEngine.Random.value < this._runChance && ((_leader == null)||_leader == this)){
						_mode = 2;
					}
				}
			}
	
		if(_reachedWaypoint && _mode > 0){
			_waypoint = t;
			CancelInvoke("WalkTimeOut");
			Invoke("WalkTimeOut", 30.0f);
			_reachedWaypoint = false;
		}
		
		_waypoint.y = _collider.transform.position.y;
		_lerpCounter = 0;	
	}
	
	public void Init() {
		if(_controller != null){
			//Controller holds all particle effects, greatly reduces draw calls compared to using particle systems on everything
			InvokeRepeating("Effects" , 1+UnityEngine.Random.value ,.1f);
		}
		//Repeating function: creates new waypoits and sets behavior
		InvokeRepeating("Wander" , 1+UnityEngine.Random.value ,1.0f);
		
		//Repeating function: Finds and sets ground position
		InvokeRepeating("GroundCheck", (_groundCheckInterval*UnityEngine.Random.value) +1 , _groundCheckInterval);
		
		//Repeating function: Looks for a herd leader, or sets itself as one if none are found
		InvokeRepeating("FindLeader", UnityEngine.Random.value*3, 3.0f);
	}
	
	float AngleAmount() {
		Vector3 dir = (_waypoint - transform.position).normalized;

		float direction = Vector3.Dot (dir, transform.right);
		float behind = Vector3.Dot (dir, transform.forward);
		if (behind < 0) {
			if (direction < 0) direction = -1;
			if (direction > 0) direction = 1;
		}
		return direction;
	}

	public void AnimationHandler(){
		if(!_dead){
			if(_mode == 1){
				if(_speed>0)
					_model.GetComponent<Animation>()[_animWalk].speed = (_speed*_animWalkSpeed) + 0.051f; 
				else
					_model.GetComponent<Animation>()[_animWalk].speed = .1f;
				_model.GetComponent<Animation>().CrossFade(_animWalk, .5f);
				_idle = false;
			}else if(_mode == 2){		
				if(_speed > _runSpeed*.35f){
				_model.GetComponent<Animation>().CrossFade(_animRun, .5f);
				_model.GetComponent<Animation>()[_animRun].speed = (_speed*_animRunSpeed) + 0.051f;
				}else{
				_model.GetComponent<Animation>().CrossFade(_animWalk, .5f);
				_model.GetComponent<Animation>()[_animWalk].speed = (_speed*_animWalkSpeed) + 0.051f;
				}
				_idle = false;	
				
			}else{
				if(!_idle && _speed < .5f){
					_sleepCounter = 0.0f;
					_model.GetComponent<Animation>().CrossFade(_animIdle, 1.0f);
					_idle = true;					
				}
				if(_idle && _sleepCounter > _idleToSleepSeconds)
					_model.GetComponent<Animation>().CrossFade(_animSleep, 1.0f);
				else 
					_sleepCounter+=_newDelta;
			}

		}
	}
	
	//Scare this for x seconds, t = object this is running away from
	public void Scare(Transform t){
	    if(_scaredOf == null)
	    _scaredOf = t;
	    _mode = 2;
	    if(!_scared){
	        _scared = true;
	        UnFlock();       
			Invoke("EndScare", 3.0f);	
	    }else{   
	        if(Vector3.Distance(_scaredOf.position, _thisTR.position) > Vector3.Distance(t.position, _thisTR.position)){       
	            _scaredOf = t;   
	        }       
	    }
	}
	
	public void EndScare(){
		_scared = false;
		Wander();
		_reachedWaypoint = true;
	}
	
	//
	public void Food(Transform t){
		if(_food == null){
			_food = t;
		}
	}
	
	//Uses scanner to push away from obstacles
	public void Pushy() {
		RaycastHit hit = new RaycastHit();
		float dx = 0.0f;
		Vector3 fwd = _scanner.forward;
	
		if(_scan)	//Scan fast if not pushing
		_scanner.Rotate(new Vector3(0.0f,1000*_newDelta,0.0f));
		else		//Scan slow if pushing
		_scanner.Rotate(new Vector3(0.0f,250*_newDelta,0.0f));
		if (Physics.Raycast(_collider.transform.position, fwd, out hit, _pushDistance, _pushyLayerMask)){
			Transform hitTransform = hit.transform;
			if(hitTransform.gameObject.layer != _groundIndex||(hitTransform.gameObject.layer == _groundIndex && Vector3.Angle(Vector3.up, hit.normal) > _maxGroundAngle)){	
				float dist = hit.distance;
				dx = (_pushDistance - dist)/_pushDistance;	
				if(gameObject.layer != _herdSimIndex){	
					_thisTR.position -= fwd*_newDelta*dx*_pushForce;
				}else if(dist < _pushDistance *.5f){
					_thisTR.position -= fwd*_newDelta*(dx-.5f)*_pushForce;				
				}
				_scan = false;
			}else{
				_scan = true;
			}
		}else{
			_scan = true;
		}
	}
	
	public void UnFlock(){
		if((_leader != null) && _leader != this){
			_reachedWaypoint = true;
			_leader._leaderSize--;
			_leader = null;
			Wander();
		}
	}
	
	public void WalkTimeOut() {
		_reachedWaypoint = true;
		UnFlock();
		Wander();
	}
	
	public void Update() {
		//Skip frames
		if(_updateDivisor > 1){
			_updateCounter++;
			if (_updateCounter != _updateSeed ){
		            _updateCounter = _updateCounter % _updateDivisor;	
		            return;         
		        }
		        _updateCounter = _updateCounter % _updateDivisor;
			_newDelta = Time.deltaTime*_updateDivisor;	
		}else{
			_newDelta = Time.deltaTime;
		}
		//Fake collisions
		if ((!_pushHalfTheTime || _pushToggle) && _mode > 0)
		Pushy();
		_pushToggle=!_pushToggle;
		
		//Fake gravity
		Vector3 gr = _thisTR.position;
		gr.y -= (_thisTR.position.y-_ground.y)*_newDelta*_fakeGravity;
		_thisTR.position = gr;
		
		if(!_dead){
			AnimationHandler();
		
		Vector3 lookit = Vector3.zero;
		Quaternion rotation = Quaternion.identity;
		
		//Rotates model to align with the ground
		_model.transform.rotation = Quaternion.Slerp(_model.transform.rotation, _groundRot, _newDelta  * 5);
		Quaternion rot = _model.transform.localRotation;
		rot.eulerAngles = new Vector3(rot.eulerAngles.x, 0.0f, rot.eulerAngles.y);
		_model.transform.localRotation = rot;
		
		//Look at waypoints, rotation is used for movement direction
		if(!_scared && _mode>0){
			lookit = _waypoint - _thisTR.position;
			if(lookit != Vector3.zero) rotation = Quaternion.LookRotation(lookit);
		}
		else if(_scared && (_scaredOf != null)){
			lookit = _scaredOf.position - _thisTR.position;
			if(lookit != Vector3.zero) rotation = Quaternion.LookRotation(-lookit);
		}
	  
	    
	    //Check distance to waypoint
	    if((_thisTR.position - _waypoint).sqrMagnitude < 10){
	   		if(_mode > 0)
	   		_mode=1;
	   		_reachedWaypoint = true;
	    }else{
	    	_eating = false;
	    }
	    
	    //If scared this will always run. Eating always stopped if not scared.
	   	if(this._scared||(_leader != null) && _leader!=this && _leader._mode==2){
	   		_mode = 2;
	   	}else if(_eating){
	   		_mode = 0;
	   	}
	   	 	
	    if(_mode == 1){   
	    	 if(_leader !=this)
	    	 _targetSpeed = _walkSpeed;
	    	 else
	    	 _targetSpeed = _walkSpeed*.75f;
	    }else if(_mode == 2){
	    	_targetSpeed = _runSpeed;
	    }
	    
	  	_speed = Mathf.Lerp(_speed, _targetSpeed, _lerpCounter * _newDelta *.05f);
	  	_lerpCounter++;
	
	    if(_speed > 0.01f && !Avoidance ()){
	   	 	_thisTR.rotation = Quaternion.Slerp(_thisTR.rotation, rotation, _newDelta * _damping);
	    }
	    
	  	if(_mode == 1) _targetSpeed = _walkSpeed;
	    else if(_mode == 2) _targetSpeed = _runSpeed;
	    else if(_mode <= 0) _targetSpeed = 0.0f;
	    
		_thisTR.rotation = Quaternion.Euler(0.0f, _thisTR.rotation.eulerAngles.y,0.0f);

			Lean();

		}

		///MOVEMENT
		if (!_grounded){
			//Turns off scared when not grounded
			_scared = false;
			UnFlock();
			Vector3 pos = Vector3.zero;
			pos = _thisTR.position;
			pos.x -= (_thisTR.position.x-_ground.x)*_newDelta*15;
			pos.z -= (_thisTR.position.z-_ground.z)*_newDelta*15;
			_thisTR.position = pos;
		}else if(!_dead){	
	    	_thisTR.position += _thisTR.TransformDirection(Vector3.forward)*_speed*_newDelta;	
		}
		
	}
	
	public void GroundCheck(){
		RaycastHit hit = new RaycastHit();
		if (Physics.Raycast(new Vector3(_thisTR.position.x, _collider.transform.position.y, _thisTR.position.z) , -_thisTR.up, out hit, _maxFall, _groundLayerMask)){
			_grounded = true;
			_groundRot = Quaternion.FromToRotation(_model.transform.up, hit.normal) * _model.transform.rotation;
			_ground = hit.point;
		}else{
			_grounded = false;
			_waypoint = _thisTR.position + (_thisTR.right*5);
			_speed = 0.0f;
		}
	}
	  		
	void NotAvoiding() {
		_avoidingRight = _avoidingLeft = false;
	}
				
	public bool Avoidance() {
		//Avoidance () - Returns true if there is an obstacle in the way
			bool r = false;
			RaycastHit hit = new RaycastHit();
			float dx = 0.0f;
			Vector3 fwd = _model.transform.forward;
			Vector3 rgt = _model.transform.right;
			Transform hitTransform = null;
			float spd = Mathf.Clamp(_speed, 0.5f, 1.0f);
			Quaternion rot = Quaternion.identity;
			
			//If idle and not moving return
			if(_mode == 0 && _speed < 0.21f){
				return true;
			}

		

		//Avoid obstacles left and right in front
		if (_mode > 0 && _rotateCounterR == 0 && Physics.Raycast(_collider.transform.position, fwd+(rgt*(_avoidAngle+_rotateCounterL)), out hit, _avoidDistance,_pushyLayerMask)){
				hitTransform = hit.transform;
				if(hitTransform.gameObject.layer != _groundIndex ||(hitTransform.gameObject.layer == _groundIndex && Vector3.Angle(Vector3.up, hit.normal) > _maxGroundAngle)){	
					//	Debug.DrawLine(_collider.transform.position,hit.point);
					_rotateCounterL+=_newDelta;
					dx = (_avoidDistance - hit.distance)/_avoidDistance;
					rot = _thisTR.rotation;	
					rot.eulerAngles = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y -_avoidSpeed*_newDelta*dx*_rotateCounterL*spd, rot.eulerAngles.z);
					_thisTR.rotation = rot;
				


				CancelInvoke("NotAvoiding");
				Invoke("NotAvoiding", .5f);
				_avoidingLeft = true;
				_avoidingRight = false;

				if (_rotateCounterL > 1.5f){
						_rotateCounterL = 1.5f;	
						_rotateCounterR = 0.0f;
						r= true;
					}
				}
			}
			else if (_mode > 0 && _rotateCounterL == 0 && Physics.Raycast(_collider.transform.position, fwd+(rgt*-(_avoidAngle+_rotateCounterR)), out hit, _avoidDistance,_pushyLayerMask)){
				hitTransform = hit.transform;
				if(hitTransform.gameObject.layer != _groundIndex  ||(hitTransform.gameObject.layer == _groundIndex && Vector3.Angle(Vector3.up, hit.normal) > _maxGroundAngle)){
				//	Debug.DrawLine(_collider.transform.position,hit.point);
					_rotateCounterR +=_newDelta;
					dx = (_avoidDistance - hit.distance)/_avoidDistance;
					rot = _thisTR.rotation;	
					rot.eulerAngles = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y + _avoidSpeed*_newDelta*dx*_rotateCounterR*spd, rot.eulerAngles.z);

				CancelInvoke("NotAvoiding");
				Invoke("NotAvoiding", .5f);
				_avoidingLeft = false;
				_avoidingRight = true;

				if (_rotateCounterR > 1.5f){
						_rotateCounterR = 1.5f;
						_rotateCounterL = 0.0f;
						r= true;
					}
				}
			}else{
			_rotateCounterL -= _newDelta;
				if(_rotateCounterL < 0) _rotateCounterL = 0.0f;
				_rotateCounterR -= _newDelta;
				if(_rotateCounterR < 0) _rotateCounterR = 0.0f;
			}
			//Crash avoidance //Checks for obstacles forward
			if (Physics.Raycast(_collider.transform.position, fwd+(rgt*UnityEngine.Random.Range(-.1f, .1f)), out hit, _avoidDistance *.9f,_pushyLayerMask)){
				hitTransform = hit.transform;
				if(hitTransform.gameObject.layer != _groundIndex ||(hitTransform.gameObject.layer == _groundIndex && Vector3.Angle(Vector3.up, hit.normal) > _maxGroundAngle)){
					//Debug.DrawLine(_collider.transform.position,hit.point);
					float dist = hit.distance;
					dx = (_avoidDistance - hit.distance)/_avoidDistance;
					rot = _thisTR.rotation;			
					if(_rotateCounterL > _rotateCounterR){
							rot.eulerAngles = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y -_avoidSpeed*_newDelta*dx*_rotateCounterL, rot.eulerAngles.z);
						}
						else{
							rot.eulerAngles = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y + _avoidSpeed*_newDelta*dx*_rotateCounterR, rot.eulerAngles.z);
						}
					transform.rotation = rot;
				
				if (dist < _stopDistance*.5f){	
						_speed = -.2f;
						r = true;
					}if(dist < _stopDistance && _speed > .2f){
						_speed -= _newDelta*(1-dx)*25;
					}
					
					if(_speed < -.2f){
						_speed = -.2f;
					}
				}
			}

		if (r) {
			_avoiding = true;
		} else {
			_avoiding = false;
		}


		return r;																	    																																				    																				
	}
	
	public void OnDrawGizmos() {
			GUIStyle guiStyle = new GUIStyle();
			Color colorBlue = Color.blue;
			Color colorCyan = new Color32((byte)0, (byte)255, (byte)246, (byte)255);
			Color colorYellow = new Color32((byte)255, (byte)255, (byte)0, (byte)255);
			
			guiStyle.normal.textColor = Color.yellow;		
			if(!Application.isPlaying){
				_startPosition = transform.position;			
			}else{
			 	Gizmos.color = colorCyan;
		  		Gizmos.DrawLine(_collider.transform.position, _waypoint);
			}
			if(_controller == null){
		       	Gizmos.color = colorBlue;
		       	Gizmos.DrawWireCube (_startPosition, _roamingArea*2);
			}
			if(_leader==this){
				Gizmos.color = colorYellow;
		       	Gizmos.DrawWireCube (_thisTR.position, new Vector3(_leaderArea.x*2, 0.0f, _leaderArea.y*2));
				Gizmos.DrawIcon(_collider.transform.position,"leader.png", false);
				//Handles.Label(_collider.transform.position, " " +this._leaderSize + " / " + this._herdSize,g);
			}else if(_leader != null){
				Gizmos.color = colorYellow;
				Gizmos.DrawLine(_collider.transform.position, _leader._collider.transform.position);
			}	
			if(_scared)
			Gizmos.DrawIcon(_collider.transform.position,"scared.png", false);
		
		if(_dead){
			Gizmos.DrawIcon(_collider.transform.position,"dead.png", false);
		}	
	}


//function LerpByDistance( A:Vector3, B:Vector3, x:float)
//{
//     var P:Vector3 = x * Vector3.Normalize(B - A) + A;
//    return P;
//}
}