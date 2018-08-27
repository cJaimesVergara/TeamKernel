using UnityEngine;
using System;


public class HerdSimDisabler:MonoBehaviour{
	public int _distanceDisable = 1000;
	public Transform _distanceFrom;
	public bool _distanceFromMainCam;
	public float _checkDisableEverSeconds = 10.0f;
	public float _checkEnableEverSeconds = 1.0f;
	
	public bool _disableModel;
	public bool _disableCollider;
	public bool _disableOnStart;
	
	public void Start() {
		if(_distanceFromMainCam){
			_distanceFrom = Camera.main.transform;
		}
		
		InvokeRepeating("CheckDisable", _checkDisableEverSeconds + (UnityEngine.Random.value * _checkDisableEverSeconds) , _checkDisableEverSeconds);
		InvokeRepeating("CheckEnable", _checkEnableEverSeconds + (UnityEngine.Random.value * _checkEnableEverSeconds) , _checkEnableEverSeconds);
		
		Invoke("DisableOnStart", 0.01f);
	}
	
	public void DisableOnStart() {
		if(_disableOnStart){
			transform.GetComponent<HerdSimCore>().Disable(_disableModel, _disableCollider);
		}
	}
	
	public void CheckDisable() {
		if(_distanceFrom != null && transform.GetComponent<HerdSimCore>()._enabled && (transform.position - _distanceFrom.position).sqrMagnitude > _distanceDisable){
			transform.GetComponent<HerdSimCore>().Disable(_disableModel, _disableCollider);	
		}
	}
	
	public void CheckEnable() {
		if(_distanceFrom != null && !transform.GetComponent<HerdSimCore>()._enabled && (transform.position - _distanceFrom.position).sqrMagnitude < _distanceDisable){	
			transform.GetComponent<HerdSimCore>().Enable();
		}
	}
}