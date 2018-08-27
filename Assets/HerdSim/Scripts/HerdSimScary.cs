using UnityEngine;
using System;


public class HerdSimScary:MonoBehaviour{
	public HerdSimCore _chase;
	public int[] _scareType;			//What types can this scare
	public bool _canChase;			//If this is a HerdSim object, it will chase others
	
	public float _scaryInterval = .25f;
	
	public LayerMask _herdLayerMask = (LayerMask)(-1);
	
	public void Start() {
		Init();
	}
	
	public void Init() {
		if(_scareType.Length > 0){
			InvokeRepeating("BeScary", (UnityEngine.Random.value*_scaryInterval)+1 , _scaryInterval);
			InvokeRepeating("CheckChase", 2.0f,2.0f);
		}else{
			Debug.Log(this.transform.name + " has nothing to scare; Please assigne ScareType");
		}
	}
	
	
	public void CheckChase(){
		_canChase=!_canChase;
		if(!_canChase)
		_chase = null;
	}
	
	public void BeScary() {
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, 4.0f, _herdLayerMask);
		HerdSimCore c = null;
		for(int i = 0; i < hitColliders.Length; i++) {
			Transform t = hitColliders[i].transform.parent;
			if(t != null)
			c = t.GetComponent<HerdSimCore>();
			if(c != null){
				bool scare = false;
				for(int j = 0; j < _scareType.Length; j++){		
					if(c._type == _scareType[j])
					scare=true;			
				}
				if(scare){
					c.Scare(this.transform);
					if((_chase == null) && _canChase)
					_chase = c;
				}
			}	
		}
		if(_chase != null){
			HerdSimCore p = GetComponent<HerdSimCore>();
			if(p != null){
				p._waypoint = _chase.transform.position;
				p._mode = 2;
			}
		}
	}
}